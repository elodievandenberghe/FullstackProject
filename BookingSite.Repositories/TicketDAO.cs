using BookingSite.Domains.Context;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class TicketDAO : ITicketDAO
{
    private readonly Context _dbContext;

    public TicketDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Ticket entity)
    {
        try
        {
            await _dbContext.Tickets.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Ticket entity)
    {
        try
        {
            _dbContext.Tickets.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Ticket?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Tickets.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Ticket>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.Tickets
                .Include(b => b.Flight)
                .Include(b => b.Seat)
                .Include(b => b.Meal)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Ticket entity)
    {
        try
        {
            _dbContext.Tickets.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Ticket?> GetBySeatId(int id)
    {
        try
        {
            return await _dbContext.Tickets.Where(t => t.SeatId == id)?.FirstAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            return null;
        }    
    }
}