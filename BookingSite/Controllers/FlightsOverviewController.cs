using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Extension;
using BookingSite.Services;
using BookingSite.Services.Interfaces;
using BookingSite.Utils.DatabaseLogicFunctions;
using BookingSite.Utils.Exceptions;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Net.Sockets;

namespace BookingSite.Controllers;

public class FlightsOverviewController : Controller
{
    private IFlightService _flightService;
    private IMealChoiceService _mealService;
    private ISeasonService _seasonService;
    private ITicketService _ticketService;
    //private IService<TravelClass, int> _travelClassService;
    private FlightCapacityChecker _flightCapacityChecker;
    private ILogger<FlightsOverviewController> _logger;

    private static readonly DateTime MIN_DATE = DateTime.Today.AddDays(3);
    private static readonly DateTime MAX_DATE = DateTime.Today.AddMonths(6);

    private readonly IMapper _mapper;

    public FlightsOverviewController(IMapper mapper, IFlightService flightService, ISeasonService seasonService,
        IMealChoiceService mealService, ITicketService ticketService, ILogger<FlightsOverviewController> logger)
    {
        _mapper = mapper;
        _flightService = flightService;
        _mealService = mealService;
        _seasonService = seasonService;
        //_travelClassService = travelServiceService;
        _ticketService = ticketService;
        _flightCapacityChecker = new FlightCapacityChecker(flightService, ticketService);
        _logger = logger;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        try
        {
            var lstFlights = await _flightService.GetAllAsync();
            if (lstFlights != null)
            {
                var flightViewModels = _mapper.Map<List<FlightViewModel>>(
                    lstFlights.Where(f =>
                        f.Date.ToDateTime(TimeOnly.MinValue) >= MIN_DATE &&
                        f.Date.ToDateTime(TimeOnly.MinValue) <= MAX_DATE)
                );

                // For each flight, check availability and set capacity info
                foreach (var flight in flightViewModels)
                {
                    try
                    {
                        var availableSeats = await _flightCapacityChecker.GetAvailableSeats(flight.Id);
                        flight.AvailableSeats = availableSeats;
                    }
                    catch (NoPlaneAssignedException)
                    {
                        flight.AvailableSeats = 0;
                    }
                }
                
                return View(flightViewModels);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("errorlog" + ex.Message);
        }

        return View();
    }

    [Authorize]
    public async Task<IActionResult> Buy(int id)
    {
        var flight = await _flightService.FindByIdAsync(id);
        if (flight == null)
        {
            return NotFound("Flight not found");
        }

        // Check booking window constraints
        DateTime flightDate = flight.Date.ToDateTime(TimeOnly.MinValue);
        if (flightDate < MIN_DATE || flightDate > MAX_DATE)
        {
            return NotFound("Booking is only available from 6 months before until 3 days before departure");
        }

        var ticketOverviewVmViewModel = await GetTicketOverviewViewModelAsync(id);
        if (ticketOverviewVmViewModel == null)
        {
            return NotFound("Flight not found");
        }

        return View(ticketOverviewVmViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CalculateTickets([FromBody] TicketCalculationRequest request)
    {
        if (request == null || request.FlightId <= 0)
            return BadRequest("Invalid request");

        var flight = await _flightService.FindByIdAsync(request.FlightId);
        if (flight == null)
            return NotFound("Flight not found");

        try
        {
            var tickets = request.Tickets ?? new List<TicketSelectionData>();

            // If AddAction is true, append a placeholder ticket for server-side validation
            if (request.AddAction)
            {
                tickets = new List<TicketSelectionData>(tickets)
            {
                new TicketSelectionData
                {
                    SeatClassId = ((int)SeatClass.FirstClass).ToString(),
                    MealId = null
                }
            };
            }

            var calculationResult = await CalculateTicketPricesAsync(flight, tickets);

            // If AddAction, return only the last ticket's availability (for the new ticket)
            if (request.AddAction)
            {
                var canAdd = calculationResult.Count > request.Tickets.Count &&
                             calculationResult.Last().AvailableClasses.Any();
                return Json(new { canAdd, tickets = calculationResult });
            }

            return Json(calculationResult);
        }
        catch (NoPlaneAssignedException)
        {
            return NotFound("No plane assigned to this flight");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating tickets");
            return StatusCode(500, "An error occurred while calculating tickets");
        }
    }

    /// <summary>
    /// Calculates ticket prices and availability for a given flight and ticket selection.
    /// </summary>
    public async Task<List<TicketCalculationResponse>> CalculateTicketPricesAsync(Flight flight, List<TicketSelectionData> tickets, bool addShoppingCartTickets = true)
    {
        if (flight?.Plane == null)
            throw new NoPlaneAssignedException("No plane is assigned to this flight");

        int firstClassCapacity = flight.Plane.FirstClassCapacity;
        int secondClassCapacity = flight.Plane.SecondClassCapacity;

        var bookedTickets = await _ticketService.GetByFlightIdAsync(flight.Id) ?? throw new Exception("Tickets to check remaining capacity not found");

        // Add Tickets already in the shopping carts
        if (addShoppingCartTickets)
        {
            var shopping = HttpContext.Session.GetObject<CartModel>("ShoppingCart") ?? new CartModel();
            foreach (var item in shopping.Carts)
            {
                bookedTickets = bookedTickets.Append(new Ticket
                {
                    SeatClass = item.SeatClass,
                    IsCancelled = false
                });
            }
        }

        int bookedFirstClass = bookedTickets.Count(t => t.SeatClass == SeatClass.FirstClass && !t.IsCancelled);
        int bookedSecondClass = bookedTickets.Count(t => t.SeatClass == SeatClass.SecondClass && !t.IsCancelled);

        // Calculate initial availability
        int firstClassAvailable = firstClassCapacity - bookedFirstClass;
        int secondClassAvailable = secondClassCapacity - bookedSecondClass;

        var response = new List<TicketCalculationResponse>();
        var validTickets = new List<TicketSelectionData>();

        // First pass: Ensure classes are valid based on availability and adjust if needed
        foreach (var ticket in tickets)
        {
            int selectedClass = !ticket.SeatClassId.IsNullOrEmpty()
                ? Convert.ToInt32(ticket.SeatClassId)
                : (int)SeatClass.SecondClass;

            // Check if the selected class is still available
            if (selectedClass == (int)SeatClass.FirstClass && firstClassAvailable <= 0)
            {
                // First class not available, try second class
                if (secondClassAvailable > 0)
                {
                    selectedClass = (int)SeatClass.SecondClass;
                    secondClassAvailable--;
                    validTickets.Add(ticket); // Add to our valid tickets
                }
                else
                {
                    // No seats available at all, don't add this ticket or any subsequent ones
                    break;
                }
            }
            else if (selectedClass == (int)SeatClass.SecondClass && secondClassAvailable <= 0)
            {
                // Second class not available, try first class
                if (firstClassAvailable > 0)
                {
                    selectedClass = (int)SeatClass.FirstClass;
                    firstClassAvailable--;
                    validTickets.Add(ticket); // Add to our valid tickets
                }
                else
                {
                    // No seats available at all, don't add this ticket or any subsequent ones
                    break;
                }
            }
            else
            {
                // Selected class is available, decrement the appropriate counter
                if (selectedClass == (int)SeatClass.FirstClass)
                    firstClassAvailable--;
                else
                    secondClassAvailable--;

                validTickets.Add(ticket); // Add to our valid tickets
            }

            // Create ticket response
            var ticketResponse = new TicketCalculationResponse
            {
                BasePrice = flight.Route.Price,
                SelectedMeal = ticket.MealId,
                SelectedClass = selectedClass,
                AvailableClasses = new List<int>() // Will be populated in second pass
            };

            // Add class upgrade fee
            if (selectedClass == (int)SeatClass.FirstClass)
            {
                ticketResponse.Fees.Add(new PriceFee
                {
                    Title = "First Class",
                    Value = flight.Route.Price * 0.5
                });
            }

            // Add season fee if applicable
            try
            {
                var seasons = await _seasonService.GetByAirportId(flight.Route.ToAirportId);
                var currentSeason = seasons?.FirstOrDefault(s =>
                    s.StartDate <= flight.Date && flight.Date <= s.EndDate);

                if (currentSeason != null)
                {
                    var seasonFee = flight.Route.Price * currentSeason.Percentage;
                    ticketResponse.Fees.Add(new PriceFee
                    {
                        Title = "Season Fee",
                        Value = seasonFee
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating season fees");
            }

            response.Add(ticketResponse);
        }

        // At this point we know which tickets we're keeping and their selected class
        // Now determine available classes for each ticket based on remaining capacity
        bool isFirstClassStillAvailable = firstClassAvailable > 0;
        bool isSecondClassStillAvailable = secondClassAvailable > 0;

        // Second pass: Set available classes based on remaining capacity
        for (int i = 0; i < response.Count; i++)
        {
            var availableClasses = new List<int>();

            // Add class options based on current availability
            if (isFirstClassStillAvailable || response[i].SelectedClass == (int)SeatClass.FirstClass)
                availableClasses.Add((int)SeatClass.FirstClass);

            if (isSecondClassStillAvailable || response[i].SelectedClass == (int)SeatClass.SecondClass)
                availableClasses.Add((int)SeatClass.SecondClass);

            response[i].AvailableClasses = availableClasses;
        }

        return response;
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToShoppingCart(MultiTicketPurchaseModel model)
    {
        if (!ModelState.IsValid)
        {
            var ticketOverviewVmViewModel = await GetTicketOverviewViewModelAsync(model.FlightId);
            if (ticketOverviewVmViewModel == null)
            {
                return NotFound("Flight not found");
            }

            return View("Buy", ticketOverviewVmViewModel);
        }

        var flight = await _flightService.FindByIdAsync(model.FlightId);
        if (flight == null)
        {
            return NotFound("Flight not found");
        }

        DateTime flightDate = flight.Date.ToDateTime(TimeOnly.MinValue);
        if (flightDate < MIN_DATE || flightDate > MAX_DATE)
        {
            return NotFound("Booking is only available from 6 months before until 3 days before departure");
        }
        // Convert tickets from the model to TicketSelectionData for price calculation
        var ticketSelectionData = model.Tickets.Select(t => new TicketSelectionData
        {
            SeatClassId = t.SeatClass,
            MealId = Convert.ToInt32(t.Meal)
        }).ToList();

        // Calculate proper pricing using the existing method that includes all fees
        var calculatedTickets = await CalculateTicketPricesAsync(flight, ticketSelectionData);

        var shopping = HttpContext.Session.GetObject<CartModel>("ShoppingCart") ?? new CartModel();

        for (int i = 0; i < model.Tickets.Count; i++)
        {
            var ticket = model.Tickets[i];
            var calculatedTicket = calculatedTickets[i];

            var selectedSeatClass = (SeatClass)Convert.ToInt32(ticket.SeatClass);

            // Calculate the total price including all fees
            double totalPrice = calculatedTicket.BasePrice;
            foreach (var fee in calculatedTicket.Fees)
            {
                totalPrice += fee.Value;
            }

            var item = new CartItem()
            {
                FlightId = flight.Id,
                MealId = Convert.ToInt32(ticket.Meal),
                SeatClass = selectedSeatClass,
                Price = totalPrice
            };

            shopping.Carts.Add(item);
        }

        HttpContext.Session.SetObject("ShoppingCart", shopping);
        return RedirectToAction("Index", "Cart");
    }

    private async Task<TicketOverviewViewModel?> GetTicketOverviewViewModelAsync(int flightId)
    {
        var flight = await _flightService.FindByIdAsync(flightId);
        if (flight == null)
        {
            return null;
        }
        var lstMealChoices = _mapper.Map<List<MealChoiceViewModel>>(await _mealService.GetByAirportId(flight.Route.ToAirportId));

        // Create seat class options
        var seatClasses = new List<SeatClassViewModel>
        {
            new() { Id = (int)SeatClass.FirstClass, Name = "First Class", Available = true },
            new() { Id = (int)SeatClass.SecondClass, Name = "Second Class", Available = true }
        };

        try
        {
            if (flight?.Plane == null)
            {
                throw new NoPlaneAssignedException("No plane is assigned to this flight");
            }

            // Check availability for each class
            var firstClassAvailable = await _flightCapacityChecker.CheckFlightAvailabilityByClass(
                flightId, SeatClass.FirstClass);
            var secondClassAvailable = await _flightCapacityChecker.CheckFlightAvailabilityByClass(
                flightId, SeatClass.SecondClass);

            // Update availability status
            seatClasses[0].Available = firstClassAvailable;
            if (!firstClassAvailable)
                seatClasses[0].Name = "First Class - Not Available";

            seatClasses[1].Available = secondClassAvailable;
            if (!secondClassAvailable)
                seatClasses[1].Name = "Second Class - Not Available";

            // If neither class is available
            if (!firstClassAvailable && !secondClassAvailable)
            {
                seatClasses.ForEach(c => {
                    c.Id = -1;
                    c.Name = "No seats available for this flight";
                    c.Available = false;
                });
            }
        }
        catch (NoPlaneAssignedException ex)
        {
            // No plane assigned to this flight
            seatClasses.ForEach(c => {
                c.Id = -1;
                c.Name = ex.Message;
                c.Available = false;
            });
        }

        var ticketOverviewVmViewModel = new TicketOverviewViewModel()
        {
            FlightId = flight.Id,
            FromAirport = flight.Route.FromAirport.Name,
            ToAirport = flight.Route.ToAirport.Name,
            RouteSegments = flight.Route.RouteSegments.OrderBy(o => o.SequenceNumber).Select(s => s.Airport.Name).ToList(),
            Date = flight.Date,
            Meals = new SelectList(lstMealChoices, "Id", "Description"),
            SeatClasses = seatClasses.Where(c => c.Available).ToList(),
            Price = flight.Route.Price,
        };
        return ticketOverviewVmViewModel;
    }
}