using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Extension;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookingSite.Controllers;

public class CartController : Controller
{
    private IMealChoiceService _mealService;
    private IService<TravelClass, int> _travelClassService;

    private readonly IMapper _mapper;

    public CartController(IMapper mapper, IMealChoiceService mealService, IService<TravelClass, int> travelServiceService)
    {
        _mapper = mapper;
        _mealService = mealService;
        _travelClassService = travelServiceService;
    }

    public async Task<IActionResult> Index()
    {
        CartViewModel? cartList = HttpContext.Session.GetObject<CartViewModel>("ShoppingCart");
        foreach (var item in cartList.Carts)
        {
            var mealDescription = await _mealService.FindByIdAsync(item.MealId);
            item.MealDescription = mealDescription.Description;
            var classType = await _travelClassService.FindByIdAsync(item.ClassId);
            item.ClassType = classType.Type;
        }
        return View(cartList);
    }
}