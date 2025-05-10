using System.Security.Claims;
using AutoMapper;
using BookingSite.Data;
using BookingSite.Domains.Models;
using BookingSite.Extension;
using BookingSite.Services;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookingSite.Utils;
using BookingSite.Utils.DatabaseLogicFunctions;
using BookingSite.Utils.Exceptions;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookingSite.Controllers;

public class CartController : Controller
{
    private FlightCapacityChecker _flightCapacityChecker;
    private IMealChoiceService _mealService;
    private ITicketService _ticketService;
    private IService<Flight, int> _flightService;
    private IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBookingService _bookingService;

    private readonly IMapper _mapper;

    public CartController(IMapper mapper, IMealChoiceService mealService, ITicketService ticketService,
        UserManager<ApplicationUser> userManager, IEmailSender emailSender, 
        IBookingService bookingService, IService<Flight, int> flightService)
    {
        _mapper = mapper;
        _mealService = mealService;
        _ticketService = ticketService;
        _userManager = userManager;
        _emailSender = emailSender;
        _bookingService = bookingService;
        _flightService = flightService;
        _flightCapacityChecker = new FlightCapacityChecker(_flightService, _ticketService);
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View(await GetList());
    }

    [Authorize]
    public async Task<IActionResult> Checkout()
    {
        return View(await GetList());
    }

    [Authorize]
    public async Task<IActionResult> PurchaseComplete()
    {
        try
        {
            CartViewModel cartlist = await GetList();
            if (cartlist.Carts != null && cartlist.Carts.Any())
            {
                // Check availability for all flights first
                foreach (var item in cartlist.Carts)
                {
                    try
                    {
                        if (!await _flightCapacityChecker.CheckFlightAvailability(item.FlightId))
                        {
                            return View("NoSeatAvailable");
                        }
                    }
                    catch (NoPlaneAssignedException)
                    {
                        return View("NoPlaneAssigned");
                    }
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var booking = new Booking
                {
                    UserId = userId
                };

                await _bookingService.AddAsync(booking);

                foreach (var item in cartlist.Carts)
                {
                    var ticket = new Ticket
                    {
                        BookingId = booking.Id,
                        FlightId = item.FlightId,
                        IsCancelled = false,
                        MealId = item.MealId,
                        SeatClass = item.SeatClass
                    };

                    await _ticketService.AddAsync(ticket);
                }

                await _emailSender.SendEmailAsync(
                    User.FindFirstValue(ClaimTypes.Email),
                    "Your booking has been completed!",
                    $"Thank you for booking with us. Your booking reference number is {booking.Id}");

                HttpContext.Session.Remove("ShoppingCart");
            }

            return View();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in PurchaseComplete: {e.Message}");
            return View("Error");
        }
    }


    public async Task<CartViewModel> GetList()
    {
        CartViewModel? cartList = HttpContext.Session.GetObject<CartViewModel>("ShoppingCart");
        if (cartList == null)
        {
            cartList = new CartViewModel();
            HttpContext.Session.SetObject("ShoppingCart", cartList);
        }
        foreach (var item in cartList.Carts)
        {
            var mealDescription = await _mealService.FindByIdAsync(item.MealId);

            if (mealDescription != null)
            {
                item.MealDescription = mealDescription.Description;
            }
            else
            {
                item.MealDescription = "Unknown Meal";
            }
            // NOT NEEDED BECAUSE ENUM
            //var classType = await _travelClassService.FindByIdAsync(item.ClassId);
            //item.ClassType = classType.Type;
        }

        return cartList;
    }
}



