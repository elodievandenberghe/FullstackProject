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
    private SeatAvailability _seatAvailability;
    private IMealChoiceService _mealService;
    private IService<TravelClass, int> _travelClassService;
    private ITicketService _ticketService;
    private ISeatService _seatService;
    private IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBookingService _bookingService;

    private readonly IMapper _mapper;

    public CartController(IMapper mapper, IMealChoiceService mealService,
        IService<TravelClass, int> travelServiceService, ITicketService ticketService
        , UserManager<ApplicationUser> userManager, ISeatService seatService, IEmailSender eaEmailSender, IBookingService bookingService)
    {
        _mapper = mapper;
        _mealService = mealService;
        _travelClassService = travelServiceService;
        _ticketService = ticketService;
        _userManager = userManager;
        _seatService = seatService;
        _emailSender = eaEmailSender;
        _seatAvailability = new SeatAvailability(seatService, ticketService);
        _bookingService = bookingService;
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
                    Console.WriteLine(item.FlightId);
                    var seat = await _seatAvailability.GetFirstAvailableSeat(item.ClassId);
                    var ticket = new Ticket()
                    {
                        BookingId = booking.Id,
                        FlightId = item.FlightId,
                        IsCancelled = false,
                        MealId = item.MealId,
                        SeatId = seat.Id
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
        catch (NoSeatAvailableException e)
        {
            return View("NoSeatAvailable");
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



