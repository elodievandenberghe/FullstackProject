using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Route = BookingSite.Domains.Models.Route;

namespace BookingSite.Controllers.API;

[Route("api/vluchten")]
public class FlightController : ControllerBase
{
     private IFlightService _flightService;
    private readonly IMapper _mapper;

    public FlightController(IMapper mapper, IFlightService flightService)
    {
        _mapper = mapper;
        _flightService = flightService;
    }

    [HttpGet("{fromAirportId}&{toAirportId}")]
    public async Task<ActionResult<RouteViewModel>> GetAll(int fromAirportId, int toAirportId)
    {
        try
        { 
            var flights = await _flightService.FindByFromAndToAirportIdAsync(fromAirportId, toAirportId);
            if (flights.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(_mapper.Map<List<FlightViewModel>>(flights));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message }); 
        }
    }
}