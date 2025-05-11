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

namespace BookingSite.Controllers;

public class FlightsOverviewController : Controller
{
    private IService<Flight, int> _flightService;
    private IMealChoiceService _mealService;
    private ISeasonService _seasonService;
    private ITicketService _ticketService;
    //private IService<TravelClass, int> _travelClassService;
    private FlightCapacityChecker _flightCapacityChecker;
    private ILogger<FlightsOverviewController> _logger;

    private readonly IMapper _mapper;

    public FlightsOverviewController(IMapper mapper, IService<Flight, int> flightService, ISeasonService seasonService,
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
                var flightViewModels = _mapper.Map<List<FlightViewModel>>(lstFlights);
                
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
        {
            return BadRequest("Invalid request");
        }

        var flight = await _flightService.FindByIdAsync(request.FlightId);
        if (flight == null)
        {
            return NotFound("Flight not found");
        }

        // Get the total capacity for each class
        int firstClassCapacity = 0;
        int secondClassCapacity = 0;

        try
        {
            if (flight?.Plane == null)
            {
                throw new NoPlaneAssignedException("No plane is assigned to this flight");
            }

            firstClassCapacity = flight.Plane.FirstClassCapacity;
            secondClassCapacity = flight.Plane.SecondClassCapacity;

            // Get already booked seats (excluding tickets in the current request)
            var bookedTickets = await _ticketService.GetByFlightIdAsync(request.FlightId);

            if (bookedTickets != null)
            {
                // Count already booked seats by class (excluding canceled tickets)
                int bookedFirstClass = bookedTickets.Count(t => t.SeatClass == SeatClass.FirstClass && !t.IsCancelled);
                int bookedSecondClass = bookedTickets.Count(t => t.SeatClass == SeatClass.SecondClass && !t.IsCancelled);

                // Calculate remaining seats
                int remainingFirstClass = firstClassCapacity - bookedFirstClass;
                int remainingSecondClass = secondClassCapacity - bookedSecondClass;

                // Calculate how many first class seats are already selected in this request
                int requestedFirstClassSeats = request.Tickets
                    .Count(t => !t.SeatClassId.IsNullOrEmpty() && Convert.ToInt32(t.SeatClassId) == (int)SeatClass.FirstClass);

                // Check if classes are available
                bool firstClassAvailable = remainingFirstClass > 0;
                bool secondClassAvailable = remainingSecondClass > 0;

                var response = new List<TicketCalculationResponse>();
                int currentFirstClassCount = 0;

                foreach (var ticket in request.Tickets)
                {
                    // Current selected class (default to second class if not specified)
                    int selectedClass = !ticket.SeatClassId.IsNullOrEmpty()
                        ? Convert.ToInt32(ticket.SeatClassId)
                        : (int)SeatClass.SecondClass;

                    // Check if the selected class is first class and if we still have capacity
                    if (selectedClass == (int)SeatClass.FirstClass)
                    {
                        if (currentFirstClassCount >= remainingFirstClass)
                        {
                            // No more first class seats, force to second class
                            selectedClass = (int)SeatClass.SecondClass;
                        }
                        else
                        {
                            // Count this ticket against first class capacity
                            currentFirstClassCount++;
                        }
                    }

                    // Calculate available classes for this ticket
                    var availableClasses = new List<int>();

                    // First class is available if we haven't allocated all remaining seats yet
                    if (firstClassAvailable && (currentFirstClassCount < remainingFirstClass))
                    {
                        availableClasses.Add((int)SeatClass.FirstClass);
                    }

                    // Second class is always available if there's capacity
                    if (secondClassAvailable)
                    {
                        availableClasses.Add((int)SeatClass.SecondClass);
                    }

                    var ticketResponse = new TicketCalculationResponse
                    {
                        BasePrice = flight.Route.Price,
                        SelectedMeal = ticket.MealId,
                        SelectedClass = selectedClass,
                        AvailableClasses = availableClasses
                    };

                    // Calculate class upgrade fee
                    if (selectedClass == (int)SeatClass.FirstClass)
                    {
                        ticketResponse.Fees.Add(new PriceFee
                        {
                            Title = "First Class",
                            Value = flight.Route.Price * 0.5 // 50% additional for first class
                        });
                    }

                    // Check if flight date is in a special season
                    try
                    {
                        var seasons = await _seasonService.GetByAirportId(flight.Route.ToAirportId);
                        var currentSeason = seasons?.FirstOrDefault(s =>
                            s.StartDate <= flight.Date && flight.Date <= s.EndDate);

                        if (currentSeason != null)
                        {
                            var seasonFee = flight.Route.Price * (currentSeason.Percentage / 100);
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

                    /*/ Add meal fee if applicable
                    if (ticket.MealId.HasValue && ticket.MealId > 0)
                    {
                        var meal = await _mealService.FindByIdAsync((int)ticket.MealId);
                        if (meal != null && meal.Price > 0)
                        {
                            ticketResponse.Fees.Add(new PriceFee
                            {
                                Title = $"Meal: {meal.Description}",
                                Value = meal.Price
                            });
                        }
                    }*/

                    response.Add(ticketResponse);
                }

                return Json(response);
            }
        }
        catch (NoPlaneAssignedException)
        {
            // Return response with no available classes
            var errorResponse = request.Tickets.Select(_ => new TicketCalculationResponse
            {
                BasePrice = flight.Route.Price,
                AvailableClasses = new List<int>(),
                Fees = new List<PriceFee> { new PriceFee { Title = "Error", Value = 0 } }
            }).ToList();

            return Json(errorResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating tickets");
            return StatusCode(500, "An error occurred while calculating tickets");
        }

        // Fallback empty response if we get here somehow
        return Json(new List<TicketCalculationResponse>());
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

        var shopping = HttpContext.Session.GetObject<CartModel>("ShoppingCart") ?? new CartModel();

        foreach (var ticket in model.Tickets)
        {
            var selectedSeatClass = (SeatClass)Convert.ToInt32(ticket.SeatClass);
            var price = flight.Route.Price * (selectedSeatClass == SeatClass.FirstClass ? 1.5 : 1.0);

            var item = new CartItem()
            {
                FlightId = flight.Id,
                MealId = Convert.ToInt32(ticket.Meal),
                SeatClass = selectedSeatClass,
                Price = price
            };

            shopping.Carts.Add(item);
        }

        HttpContext.Session.SetObject("ShoppingCart", shopping);
        return RedirectToAction("Index", "Cart");
    }

    /* REMOVE SINGLE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToShoppingCart(TicketPurchaseInputModel ticketPurchaseInputModel)
    {
        if (!ModelState.IsValid)
        {
            var ticketOverviewVmViewModel = await GetTicketOverviewViewModelAsync(ticketPurchaseInputModel.FlightId);
            if (ticketOverviewVmViewModel == null)
            {
                return NotFound("Flight not found");
            }
            ticketOverviewVmViewModel.SelectedMeal = ticketPurchaseInputModel.SelectedMeal;
            ticketOverviewVmViewModel.SelectedSeatClass = ticketPurchaseInputModel.SelectedSeatClass;
            return View("Buy", ticketOverviewVmViewModel);
        }

        var flight = await _flightService.FindByIdAsync(ticketPurchaseInputModel.FlightId);

        if (flight == null)
        {
            return NotFound("Flight not found");
        }

        var selectedSeatClass = (SeatClass)Convert.ToInt32(ticketPurchaseInputModel.SelectedSeatClass);

        var price = flight.Route.Price * (selectedSeatClass == SeatClass.FirstClass ? 1.5 : 1.0);

        var item = new CartItem()
        {
            FlightId = flight.Id,
            MealId = Convert.ToInt32(ticketPurchaseInputModel.SelectedMeal),
            SeatClass = selectedSeatClass,
            Price = price
        };

        var shopping = HttpContext.Session.GetObject<CartModel>("ShoppingCart") ?? new CartModel();
        shopping.Carts.Add(item);
        HttpContext.Session.SetObject("ShoppingCart", shopping);

        return RedirectToAction("Index", "Cart");
    }
    */

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