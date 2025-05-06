using BookingSite.Domains.Models;
using BookingSite.Services;
using BookingSite.Services.Interfaces;
using BookingSite.Utils.Exceptions;

namespace BookingSite.Utils.DatabaseLogicFunctions;

public class SeatAvailability
{
    private readonly ISeatService _seatService;
    private readonly ITicketService _ticketService;

    public SeatAvailability(ISeatService seatService, ITicketService ticketService)
    {
        _seatService = seatService;
        _ticketService = ticketService;
    }

    public async Task<Seat?> GetFirstAvailableSeat(int classId)
    {
        var seats = await _seatService.GetByClassId(classId);
        foreach (var seat in seats)
        {
            if (await _ticketService.GetBySeatId(seat.Id) == null)
            {
                return seat;
            }
        }
        throw new NoSeatAvailableException("No seat available");
    }

}