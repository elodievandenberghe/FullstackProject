using BookingSite.Domains.DatabaseContext;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class TravelClassesDAO : IDAO<TravelClass, int>
{
    private readonly Context _dbContext;

    public TravelClassesDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TravelClass entity)
    {
        try
        {
            await _dbContext.TravelClasses.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(TravelClass entity)
    {
        try
        {
            _dbContext.TravelClasses.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<TravelClass?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.TravelClasses.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<TravelClass>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.TravelClasses.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(TravelClass entity)
    {
        try
        {
            _dbContext.TravelClasses.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}