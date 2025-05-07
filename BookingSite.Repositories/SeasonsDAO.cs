using BookingSite.Domains.DatabaseContext;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class SeasonsDAO : IDAO<Season, int>
{
    private readonly Context _dbContext;

    public SeasonsDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Season entity)
    {
        try
        {
            await _dbContext.Seasons.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Season entity)
    {
        try
        {
            _dbContext.Seasons.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Season?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Seasons.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Season>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.Seasons
                .Include(b => b.Airport)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Season entity)
    {
        try
        {
            _dbContext.Seasons.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}