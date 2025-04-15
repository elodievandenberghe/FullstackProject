using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookingSite.Controllers.API;

[Route("api/luchthavens")]
public class AirportController : ControllerBase
{
    private IService<Airport, int> _airportService;
    private readonly IMapper _mapper;

    public AirportController(IMapper mapper, IService<Airport, int> airportService)
    {
        _mapper = mapper;
        _airportService = airportService;
    }

    [HttpGet]
    public async Task<ActionResult<AirportViewModel>> GetAll()
    {
        try
        {
            var data = await _airportService.GetAllAsync();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<List<AirportViewModel>>(data));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message }); 
        }
    }
}