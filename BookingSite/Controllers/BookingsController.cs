using System.Security.Claims;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSite.Controllers;

// TODO: Map to a ViewModel

[Authorize]
public class BookingsController : Controller
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
        
        return View(bookings);
    }

    public async Task<IActionResult> Details(int id)
    {
        var booking = await _bookingService.FindByIdAsync(id);
        
        if (booking == null)
        {
            return NotFound();
        }

        // Only allow the owner of the booking to view it
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (booking.UserId != userId)
        {
            return Forbid();
        }

        return View(booking);
    }
}
