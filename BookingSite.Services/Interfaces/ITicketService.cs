using BookingSite.Domains.Models;

namespace BookingSite.Services.Interfaces;

public interface ITicketService: IService<Ticket, int>
{
    Task<Ticket?> GetBySeatId(int id);
}