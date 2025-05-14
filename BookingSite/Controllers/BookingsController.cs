using System.Security.Claims;
using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingSite.Controllers;

[Authorize]
public class BookingsController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly ITicketService _ticketService;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;

    public BookingsController(IBookingService bookingService, ITicketService ticketService, IMapper mapper, IEmailSender emailSender)
    {
        _bookingService = bookingService;
        _ticketService = ticketService;
        _mapper = mapper;
        _emailSender = emailSender;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);

        var viewModel = bookings.Select(booking => new BookingWithTicketsViewModel
        {
            BookingId = booking.Id,
            UserId = booking.UserId,
            CreatedAt = booking.CreatedDate, 
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
            UserName = booking.User?.FirstName + " " + booking.User?.LastnNme,
            CreatedAt = booking.CreatedDate,
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CancelTicket(int ticketId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Get the ticket with the related flight information
        var ticket = await _ticketService.FindByIdAsync(ticketId);

        if (ticket == null)
        {
            return NotFound();
        }

        if(ticket.BookingId == null)
        {
            return Unauthorized();
        }

        // Verify that the ticket belongs to the current user
        var booking = await _bookingService.FindByIdAsync(ticket.BookingId.Value);
        if (booking == null || booking.UserId != userId)
        {
            return Unauthorized();
        }

        // Check if the ticket is already cancelled
        if (ticket.IsCancelled)
        {
            TempData["ErrorMessage"] = "This ticket is already cancelled.";
            return RedirectToAction("Details", new { id = ticket.BookingId });
        }

        // Check if the ticket has a price (is refundable)
        if (!ticket.Price.HasValue)
        {
            TempData["ErrorMessage"] = "This ticket cannot be cancelled. Please contact support for assistance.";
            return RedirectToAction("Details", new { id = ticket.BookingId });
        }

        // Check if the flight departure is more than 7 days away
        DateTime flightDate = ticket.Flight.Date.ToDateTime(TimeOnly.MinValue);
        int daysUntilFlight = (flightDate - DateTime.Today).Days;

        if (daysUntilFlight <= 7)
        {
            TempData["ErrorMessage"] = "Tickets can only be cancelled at least 7 days before departure.";
            return RedirectToAction("Details", new { id = ticket.BookingId });
        }

        // Proceed with cancellation
        ticket.IsCancelled = true;
        await _ticketService.UpdateAsync(ticket);

        // Send confirmation email
        double refundAmount = ticket.Price.Value;
        await _emailSender.SendEmailAsync(
            User.FindFirstValue(ClaimTypes.Email),
            "Your ticket has been cancelled",
            $@"Dear {booking.User?.FirstName},

Thank you for contacting us about your booking.

We confirm that your ticket (ID: {ticket.Id}) for flight {ticket.Flight.Id} from {ticket.Flight.Route.FromAirport.Name} to {ticket.Flight.Route.ToAirport.Name} on {flightDate:MMMM dd, yyyy} has been successfully cancelled.

A refund of €{refundAmount:F2} has been processed and will be credited back to your original payment method within 5-7 business days.

Booking details:
- Booking reference: {booking.Id}
- Cancelled ticket: {ticket.Id}
- Flight: {ticket.Flight.Id}
- Route: {ticket.Flight.Route.FromAirport.Name} to {ticket.Flight.Route.ToAirport.Name}
- Date: {flightDate:MMMM dd, yyyy}
- Refund amount: €{refundAmount:F2}

If you have any questions regarding your refund or booking, please contact our customer support.

Thank you for choosing our services.

Best regards,
BookingSite Team");

        TempData["SuccessMessage"] = $"Ticket cancelled successfully. A refund of €{refundAmount:F2} has been processed.";
        return RedirectToAction("Details", new { id = ticket.BookingId });
    }
}
