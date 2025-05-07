using BookingSite.Domains.Models;

namespace BookingSite.Repositories.Interfaces;

public interface ITicketDAO : IDAO<Ticket, int>
{    
    Task<IEnumerable<Ticket>?> GetByFlightIdAsync(int flightId);
}
