using System.Security.Claims;
using AutoMapper;
using BookingSite.Data;
using BookingSite.Domains.Models;
using BookingSite.Extension;
using BookingSite.Services;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookingSite.Controllers;

public class CartController : Controller
{
    private IMealChoiceService _mealService;
    private IService<TravelClass, int> _travelClassService;
    private ITicketService _ticketService;
    private ISeatService _seatService;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IMapper _mapper;

    public CartController(IMapper mapper, IMealChoiceService mealService, IService<TravelClass, int> travelServiceService, ITicketService ticketService
        , UserManager<ApplicationUser> userManager, ISeatService seatService)
    {
        _mapper = mapper;
        _mealService = mealService;
        _travelClassService = travelServiceService;
        _ticketService = ticketService;
        _userManager = userManager;
        _seatService = seatService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await GetList());
    }

    public async Task<IActionResult> Checkout()
    {
        return View(await GetList());
    }

    public async Task<IActionResult> PurchaseComplete()
    {
        CartViewModel cartlist = await GetList();
        foreach (var item in cartlist.Carts)
        {
            var seat = await GetFirstAvailableSeat(item.ClassId);
            var ticket = new Ticket()
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                FlightId = item.FlightId,
                IsCancelled = false,
                MealId = item.MealId,
                SeatId = seat.Id,
            };
            await _ticketService.AddAsync(ticket); 
        }
        return View();
    }

    public async Task<CartViewModel> GetList()
    {
        CartViewModel? cartList = HttpContext.Session.GetObject<CartViewModel>("ShoppingCart");
        foreach (var item in cartList.Carts)
        {
            var mealDescription = await _mealService.FindByIdAsync(item.MealId);
            item.MealDescription = mealDescription.Description;
            var classType = await _travelClassService.FindByIdAsync(item.ClassId);
            item.ClassType = classType.Type;
        }

        return cartList;
    }

    public async Task<Seat?> GetFirstAvailableSeat(int classId)
    {
        var seats = await _seatService.GetByClassId(classId);
        foreach (var seat in seats)
        {
            if (await _ticketService.GetBySeatId(seat.Id) == null)
            {
                return seat;
            }
        }
        return null;
    }

}



