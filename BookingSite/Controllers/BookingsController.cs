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
            CreatedAt = DateTime.Now, 
            Tickets = booking.Tickets.Select(t => new TicketPreviewViewModel
            {
                TicketId = t.Id,
                FromAirport = t.Flight.Route.FromAirport.Name,
                ToAirport = t.Flight.Route.ToAirport.Name,
                Date = t.Flight.Date.ToDateTime(TimeOnly.MinValue),
                SeatClass = t.SeatClass == SeatClass.FirstClass ? "First Class" : "Second Class",
                MealDescription = t.Meal?.Description ?? "No meal",
                Price = t.Price,
                IsCancelled = t.IsCancelled
            }).ToList()
        }).ToList();

        return View(viewModel);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var booking = await _bookingService.FindByIdAsync(id);

        if (booking == null || booking.UserId != userId)
        {
            return NotFound();
        }

        var viewModel = new BookingDetailsViewModel
        {
            Id = booking.Id,
            UserName = booking.User?.UserName,
            //CreatedAt = booking.CreatedAt,
            TotalPrice = booking.Tickets?.Sum(t => t.Price ?? 0) ?? 0,
            Tickets = booking.Tickets?.Select(t => new BookingTicketViewModel
            {
                Id = t.Id,
                SeatNumber = t.SeatNumber,
                SeatClassName = t.SeatClass.ToString(),
                IsCancelled = t.IsCancelled,
                Price = t.Price,
                MealDescription = t.Meal?.Description ?? "No meal",
                Flight = new BookingFlightViewModel
                {
                    Id = t.Flight.Id,
                    Date = t.Flight.Date,
                    Route = new BookingRouteViewModel
                    {
                        FromAirport = new AirportViewModel
                        {
                            Id = t.Flight.Route.FromAirport.Id,
                            Name = t.Flight.Route.FromAirport.Name
                        },
                        ToAirport = new AirportViewModel
                        {
                            Id = t.Flight.Route.ToAirport.Id,
                            Name = t.Flight.Route.ToAirport.Name
                        }
                    }
                }
            }).ToList() ?? new List<BookingTicketViewModel>()
        };

        return View(viewModel);
    }
}
