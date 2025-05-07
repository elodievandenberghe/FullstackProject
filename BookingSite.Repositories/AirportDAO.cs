using BookingSite.Domains.DatabaseContext;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class AirportDAO : IDAO<Airport, int>
{
    private readonly Context _dbContext;

    public AirportDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Airport entity)
    {
        try
        {
            await _dbContext.Airports.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Airport entity)
    {
        try
        {
            _dbContext.Airports.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Airport?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Airports
                .FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Airport>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.Airports
                .Include(b => b.City)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Airport entity)
    {
        try
        {
            _dbContext.Airports.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}