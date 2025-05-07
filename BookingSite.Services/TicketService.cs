using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class TicketService : ITicketService
{
    private readonly ITicketDAO _ticketDAO;

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

    public async Task<Ticket?> FindByIdAsync(int id)
    {
        return await _ticketDAO.FindByIdAsync(id);
    }

    public async Task<IEnumerable<Ticket>?> GetAllAsync()
    {
        return await _ticketDAO.GetAllAsync();
    }
    
    // Implement the new method
    public async Task<IEnumerable<Ticket>?> GetByFlightIdAsync(int flightId)
    {
        return await _ticketDAO.GetByFlightIdAsync(flightId);
    }

    public async Task UpdateAsync(Ticket entity)
    {
        await _ticketDAO.UpdateAsync(entity);
    }
}