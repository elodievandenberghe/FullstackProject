using BookingSite.Domains.Context;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class SeatDAO : IDAO<Seat, int>
{
    private readonly Context _dbContext;

    public SeatDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Seat entity)
    {
        try
        {
            await _dbContext.Seats.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Seat entity)
    {
        try
        {
            _dbContext.Seats.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Seat?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Seats.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Seat>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.Seats
                .Include(b => b.TravelClass)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Seat entity)
    {
        try
        {
            _dbContext.Seats.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}