using System.Security.Claims;
using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSite.Controllers;

[Authorize]
public class BookingsController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly ITicketService _ticketService;
    private readonly IMapper _mapper;

    public BookingsController(IBookingService bookingService, ITicketService ticketService, IMapper mapper)
    {
        _bookingService = bookingService;
        _ticketService = ticketService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);

        var viewModel = bookings.Select(booking => new BookingWithTicketsViewModel
        {
            BookingId = booking.Id,
            UserId = booking.UserId,
            CreatedAt = DateTime.Now, // You may want to add CreatedAt to your Booking model
            Tickets = booking.Tickets.Select(t => new TicketPreviewViewModel
            {
                TicketId = t.Id,
                FromAirport = t.Flight.Route.FromAirport.Name,
                ToAirport = t.Flight.Route.ToAirport.Name,
                Date = t.Flight.Date.ToDateTime(TimeOnly.MinValue),
                SeatClass = t.SeatClass == SeatClass.FirstClass ? "First Class" : "Second Class",
                MealDescription = t.Meal?.Description ?? "No meal",
                Price = (decimal?)(t.Price ?? t.Flight.Route.Price),
                IsCancelled = t.IsCancelled
            }).ToList()
        }).ToList();

        return View(viewModel);
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
