using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Extension;
using BookingSite.Services;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookingSite.Controllers;

public class CartController : Controller
{
    private IMealChoiceService _mealService;
    private IService<TravelClass, int> _travelClassService;
    private IService<Ticket, int> _ticService; 
    private IService<Booking, int> _bookingService; 
    

    private readonly IMapper _mapper;

    public CartController(IMapper mapper, IMealChoiceService mealService, IService<TravelClass, int> travelServiceService, IService<Ticket, int> tickeService,
        IService<Booking, int> bookingService)
    {
        _mapper = mapper;
        _mealService = mealService;
        _travelClassService = travelServiceService;
        _ticService = tickeService;
        _bookingService = bookingService; 
    }

    public async Task<IActionResult> Index()
    {
        return View(await getList());
    }

    public async Task<IActionResult> Checkout(CartViewModel cart)
    {
        return View(await getList());
    }

    public async Task<IActionResult> PurchaseComplete()
    {
        var ticket = new Ticket()
        {

        };
        await _ticService.AddAsync(ticket);
        var booking = new Booking()
        {

        };
        _bookingService.AddAsync(booking); 
        return View();
    }

    public async Task<CartViewModel> getList()
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
}