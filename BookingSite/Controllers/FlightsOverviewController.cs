using System.Diagnostics;
using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookingSite.Controllers;

public class FlightsOverviewController : Controller
{
    private IService<Flight, int> _flightService; 

    private readonly IMapper _mapper;

    public FlightsOverviewController(IMapper mapper, IService<Flight, int> flightService)
    {
        _mapper = mapper;
        _flightService = flightService;
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

    public IActionResult Buy(FlightViewModel flightViewModel)
    {
        return View(flightViewModel); 
    }
}