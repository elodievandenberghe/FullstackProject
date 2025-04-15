using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Route = BookingSite.Domains.Models.Route;

namespace BookingSite.Controllers.API;

[Route("api/vluchten")]
public class RouteController : ControllerBase
{
    private IRouteService _routeService;
    private readonly IMapper _mapper;

    public RouteController(IMapper mapper, IRouteService routeService)
    {
        _mapper = mapper;
        _routeService = routeService;
    }

    [HttpGet("{fromAirportId}&{toAirportId}")]
    public async Task<ActionResult<RouteViewModel>> GetAll(int fromAirportId, int toAirportId)
    {
        try
        {
            var data = await _routeService.GetByFromAirportIdToAirportId(fromAirportId, toAirportId);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<List<RouteViewModel>>(data));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message }); 
        }
    }
}