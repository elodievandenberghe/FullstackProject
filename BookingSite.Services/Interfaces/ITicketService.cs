using BookingSite.Domains.Models;

namespace BookingSite.Services.Interfaces;

public interface ITicketService : IService<Ticket, int>
{    
    Task<IEnumerable<Ticket>?> GetByFlightIdAsync(int flightId);
}