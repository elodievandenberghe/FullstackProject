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
    private IService<Flight, int> _flightService;
    private IMealChoiceService _mealService;
    //private IService<TravelClass, int> _travelClassService;
    private FlightCapacityChecker _flightCapacityChecker;

    private readonly IMapper _mapper;

    public FlightsOverviewController(IMapper mapper, IService<Flight, int> flightService, 
        IMealChoiceService mealService, ITicketService ticketService)
    {
        _mapper = mapper;
        _flightService = flightService;
        _mealService = mealService;
        //_travelClassService = travelServiceService;
        _flightCapacityChecker = new FlightCapacityChecker(flightService, ticketService);
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
    public async Task<IActionResult> Buy(FlightViewModel flightViewModel)
    {
        var lstMealChoices = _mapper.Map<List<MealChoiceViewModel>>(await _mealService.GetByAirportId(flightViewModel.ToAirportId));

        // Create seat class options
        var seatClasses = new List<SeatClassViewModel>
        {
            new() { Id = (int)SeatClass.FirstClass, Name = "First Class", Available = true },
            new() { Id = (int)SeatClass.SecondClass, Name = "Second Class", Available = true }
        };

        try
        {
            var flight = await _flightService.FindByIdAsync(flightViewModel.Id);
            if (flight?.Plane == null)
            {
                throw new NoPlaneAssignedException("No plane is assigned to this flight");
            }

            // Check availability for each class
            var firstClassAvailable = await _flightCapacityChecker.CheckFlightAvailabilityByClass(
                flightViewModel.Id, SeatClass.FirstClass);
            var secondClassAvailable = await _flightCapacityChecker.CheckFlightAvailabilityByClass(
                flightViewModel.Id, SeatClass.SecondClass);

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
            FlightId = flightViewModel.Id,
            FromAirport = flightViewModel.FromAirport,
            ToAirport = flightViewModel.ToAirport,
            RouteSegments = flightViewModel.RouteSegments,
            Date = flightViewModel.Date,
            Meals = new SelectList(lstMealChoices, "Id", "Description"),
            SeatClasses = seatClasses.Where(c => c.Available).ToList(),
            Price = flightViewModel.Price
        };

        return View(ticketOverviewVmViewModel);
    }

    [HttpPost]
    public IActionResult AddToShoppingCart(TicketOverviewViewModel ticketOverview)
    {
        var selectedSeatClass = (SeatClass)Convert.ToInt32(ticketOverview.SelectedSeatClass);

        var item = new CartItemViewModel()
        {
            FlightId = ticketOverview.FlightId,
            Date = ticketOverview.Date,
            FromAirport = ticketOverview.FromAirport,
            ToAirport = ticketOverview.ToAirport,
            RouteSegments = ticketOverview.RouteSegments,
            MealId = Convert.ToInt32(ticketOverview.SelectedMeal),
            SeatClass = selectedSeatClass,
            Price = ticketOverview.Price * (selectedSeatClass == SeatClass.FirstClass ? 1.5 : 1.0) // Apply price adjustment for first class
        };

        var shopping = HttpContext.Session.GetObject<CartViewModel>("ShoppingCart") ?? new CartViewModel() { Carts = new List<CartItemViewModel>() };
        shopping.Carts.Add(item);
        HttpContext.Session.SetObject("ShoppingCart", shopping);

        return RedirectToAction("Index", "Cart");
    }
}