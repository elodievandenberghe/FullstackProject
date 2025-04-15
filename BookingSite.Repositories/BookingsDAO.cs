using BookingSite.Domains.Context;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class BookingsDAO : IDAO<Booking, int>
{
    private readonly Context _dbContext;

    public BookingsDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Booking entity)
    {
        try
        {
            await _dbContext.Bookings.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Booking entity)
    {
        try
        {
            _dbContext.Bookings.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Booking?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Bookings.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Booking>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Ticket)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Booking entity)
    {
        try
        {
            _dbContext.Bookings.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}