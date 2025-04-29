using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class TicketService : ITicketService
{
    private ITicketDAO _ticketDAO;

    public TicketService(ITicketDAO ticketDAO) 
    {
        _ticketDAO = ticketDAO;
    }

    public async Task AddAsync(Ticket entity)
    {
        await _ticketDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(Ticket entity)
    {
        await _ticketDAO.DeleteAsync(entity);
    }

    public async Task<Ticket?> FindByIdAsync(int Id)
    {
        return await _ticketDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<Ticket>?> GetAllAsync()
    {
        return await _ticketDAO.GetAllAsync();
    }

    public async Task UpdateAsync(Ticket entity)
    {
        await _ticketDAO.UpdateAsync(entity);
    }

    public async Task<Ticket?> GetBySeatId(int id)
    {
        return await _ticketDAO.GetBySeatId(id); 
    }
}