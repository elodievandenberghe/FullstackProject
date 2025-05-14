using System.Diagnostics;
using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookingSite.Extension;
using BookingSite.Utils.DatabaseLogicFunctions;
using BookingSite.Utils.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace BookingSite.Controllers;

public class FlightsOverviewController : Controller
{
    private IFlightService _flightService;
    private IMealChoiceService _mealService;
    //private IService<TravelClass, int> _travelClassService;
    private FlightCapacityChecker _flightCapacityChecker;
    private ILogger<FlightsOverviewController> _logger;

    private readonly IMapper _mapper;

    public FlightsOverviewController(IMapper mapper, IFlightService flightService, 
        IMealChoiceService mealService, ITicketService ticketService, ILogger<FlightsOverviewController> logger)
    {
        _mapper = mapper;
        _flightService = flightService;
        _mealService = mealService;
        //_travelClassService = travelServiceService;
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