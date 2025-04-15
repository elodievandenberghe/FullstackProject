using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Services;
using Microsoft.AspNetCore.Mvc;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;

namespace BookingSite.Controllers.API;
[Route("api/gebruikers")]
public class UserController : ControllerBase
{
    private IService<AspNetUser, string> _userService;
    private readonly IMapper _mapper;

    public UserController(IMapper mapper, IService<AspNetUser, string> userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<AspNetUserViewModel>> GetAll()
    {
        try
        {
            var data = await _userService.GetAllAsync();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<List<AspNetUserViewModel>>(data));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message }); 
        }
    }
}