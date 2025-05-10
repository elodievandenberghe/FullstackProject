using BookingSite.Domains.Models;
using BookingSite.Services.Interfaces;
using BookingSite.Utils.Exceptions;

namespace BookingSite.Utils.DatabaseLogicFunctions;

public class FlightCapacityChecker
{
    private readonly IService<Flight, int> _flightService;
    private readonly ITicketService _ticketService;

    public FlightCapacityChecker(IService<Flight, int> flightService, ITicketService ticketService)
    {
        _flightService = flightService;
        _ticketService = ticketService;
    }

    public async Task<bool> CheckFlightAvailability(int flightId)
    {
        var flight = await _flightService.FindByIdAsync(flightId);
        if (flight?.Plane == null)
        {
            throw new NoPlaneAssignedException("No plane is assigned to this flight");
        }

        var tickets = await _ticketService.GetByFlightIdAsync(flightId);
        var activeTicketCount = tickets?.Count(t => t.IsCancelled != true) ?? 0;

        return activeTicketCount < flight.Plane.Capacity;
    }

    public async Task<bool> CheckFlightAvailabilityByClass(int flightId, SeatClass seatClass)
    {
        var flight = await _flightService.FindByIdAsync(flightId);
        if (flight?.Plane == null)
        {
            throw new NoPlaneAssignedException("No plane is assigned to this flight");
        }

        var tickets = await _ticketService.GetByFlightIdAsync(flightId);
        if (tickets == null) return true;

        int classCapacity = seatClass == SeatClass.FirstClass
            ? flight.Plane.FirstClassCapacity
            : flight.Plane.SecondClassCapacity;

        var activeTicketsInClass = tickets.Count(t => t.IsCancelled != true && t.SeatClass == seatClass);

        return activeTicketsInClass < classCapacity;
    }

    public async Task<int> GetAvailableSeats(int flightId)
    {
        var flight = await _flightService.FindByIdAsync(flightId);
        if (flight?.Plane == null)
        {
            throw new NoPlaneAssignedException("No plane is assigned to this flight");
        }

        var tickets = await _ticketService.GetByFlightIdAsync(flightId);
        var activeTicketCount = tickets?.Count(t => t.IsCancelled != true) ?? 0;

        return flight.Plane.Capacity - activeTicketCount;
    }

    public async Task<int> GetAvailableSeatsByClass(int flightId, SeatClass seatClass)
    {
        var flight = await _flightService.FindByIdAsync(flightId);
        if (flight?.Plane == null)
        {
            throw new NoPlaneAssignedException("No plane is assigned to this flight");
        }

        var tickets = await _ticketService.GetByFlightIdAsync(flightId);

        int classCapacity = seatClass == SeatClass.FirstClass
            ? flight.Plane.FirstClassCapacity
            : flight.Plane.SecondClassCapacity;

        var activeTicketsInClass = tickets?.Count(t => t.IsCancelled != true && t.SeatClass == seatClass) ?? 0;

        return classCapacity - activeTicketsInClass;
    }
}