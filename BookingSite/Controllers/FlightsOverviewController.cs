using System.Diagnostics;
using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            Meals = new SelectList(lstMealChoices, "Id", "Description"), 
            Classes = new SelectList(lstClasses, "Id", "Type"), 
            Price =flightViewModel.Price
        };
        
        return View(ticketOverviewVmViewModel); 
    }

    public IActionResult ShoppingCart()
    {
        return View();
    }
}