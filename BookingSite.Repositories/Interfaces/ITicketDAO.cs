using BookingSite.Domains.Models;

namespace BookingSite.Repositories.Interfaces;

public interface ITicketDAO: IDAO<Ticket, int>
{
    Task<Ticket?> GetBySeatId(int id);
}