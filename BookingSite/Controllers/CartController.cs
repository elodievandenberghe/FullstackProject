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
    private IService<TravelClass, int> _travelClassService;
    private ITicketService _ticketService;
    private IService<Flight, int> _flightService;
    private IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBookingService _bookingService;

    private readonly IMapper _mapper;

    public CartController(IMapper mapper, IMealChoiceService mealService,
        IService<TravelClass, int> travelServiceService, ITicketService ticketService,
        UserManager<ApplicationUser> userManager, IEmailSender emailSender, 
        IBookingService bookingService, IService<Flight, int> flightService)
    {
        _mapper = mapper;
        _mealService = mealService;
        _travelClassService = travelServiceService;
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
                // Create a single booking for all tickets in the cart
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var booking = new Booking
                {
                    UserId = userId
                };

                // Add booking to database to get its ID
                await _bookingService.AddAsync(booking);

                foreach (var item in cartlist.Carts)
                {
                    // Check if the flight has available seats
                    bool isAvailable;
                    try 
                    {
                        isAvailable = await _flightCapacityChecker.CheckFlightAvailability(item.FlightId);
                    }
                    catch (NoPlaneAssignedException) 
                    {
                        return View("NoPlaneAssigned");
                    }
                    
                    if (!isAvailable) 
                    {
                        return View("NoSeatAvailable");
                    }

                    // Create ticket - seat number will be assigned automatically in TicketDAO
                    var ticket = new Ticket()
                    {
                        BookingId = booking.Id,
                        FlightId = item.FlightId,
                        IsCancelled = false,
                        MealId = item.MealId
                        // SeatNumber will be assigned in the DAO
                    };
                    
                    await _ticketService.AddAsync(ticket);
                }

                _emailSender.SendEmailAsync(User.FindFirstValue(ClaimTypes.Email), "Your booking has been completed!",
                    "Thank you for booking with us. Your booking reference number is " + booking.Id);

                // Clear the shopping cart after successful purchase
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
            item.MealDescription = mealDescription.Description;
            var classType = await _travelClassService.FindByIdAsync(item.ClassId);
            item.ClassType = classType.Type;
        }

        return cartList;
    }
}



