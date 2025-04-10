using BookingSite.Domains.Context;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class FlightDAO : IDAO<Flight, int>
{
    private readonly Context _dbContext;

    public FlightDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Flight entity)
    {
        try
        {
            await _dbContext.Flights.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Flight entity)
    {
        try
        {
            _dbContext.Flights.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Flight?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Flights.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Flight>> GetAllAsync()
    {
        try
        {
            return await _dbContext.Flights
                .Include(b => b.Route)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Flight entity)
    {
        try
        {
            _dbContext.Flights.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}