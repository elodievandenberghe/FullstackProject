using System.Diagnostics;
using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookingSite.Extension;

namespace BookingSite.Controllers;

public class FlightsOverviewController : Controller
{
    private IService<Flight, int> _flightService;
    private IMealChoiceService _mealService;
    private IService<TravelClass, int> _travelClassService;

    private readonly IMapper _mapper;

    public FlightsOverviewController(IMapper mapper, IService<Flight, int> flightService, 
        IMealChoiceService mealService, IService<TravelClass, int> travelServiceService)
    {
        _mapper = mapper;
        _flightService = flightService;
        _mealService = mealService;
        _travelClassService = travelServiceService; 
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var lstFlights = await _flightService.GetAllAsync();
            if (lstFlights != null)
            {
                return View(_mapper.Map<List<FlightViewModel>>(lstFlights));
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("errorlog" + ex.Message);
        }

        return View(); 
    }

    public async Task<IActionResult> Buy(FlightViewModel flightViewModel)
    {
        var lstMealChoices = _mapper.Map<List<MealChoiceViewModel>>(await _mealService.GetByAirportId(flightViewModel.ToAirportId));
        var lstClasses = _mapper.Map<List<TravelClassViewModel>>(await _travelClassService.GetAllAsync());
        var ticketOverviewVmViewModel = new TicketOverviewViewModel()
        {
            FromAirport = flightViewModel.FromAirport,
            ToAirport = flightViewModel.ToAirport,
            RouteSegments = flightViewModel.RouteSegments,
            Date = flightViewModel.Date,
            Meals = new SelectList(lstMealChoices, "Id", "Description"), 
            Classes = new SelectList(lstClasses, "Id", "Type"), 
            Price =flightViewModel.Price
        };
        
        return View(ticketOverviewVmViewModel); 
    }
    [HttpPost]
    public IActionResult AddToShoppingCart(TicketOverviewViewModel ticketOverview)
    {
        var item = new CartItemViewModel()
        {
            Date = ticketOverview.Date,
            FromAirport = ticketOverview.FromAirport,
            ToAirport = ticketOverview.ToAirport,
            RouteSegments = ticketOverview.RouteSegments,
            MealId = Convert.ToInt32(ticketOverview.SelectedMeal),
            ClassId = Convert.ToInt32(ticketOverview.SelectedClass),
            Price = ticketOverview.Price
        };
        var shopping = HttpContext.Session.GetObject<CartViewModel>("ShoppingCart" ) ?? new CartViewModel() { Carts = new List<CartItemViewModel>() };
        shopping.Carts.Add(item);
        HttpContext.Session.SetObject("ShoppingCart", shopping);
        
        return RedirectToAction("Index", "Cart");
    }
}