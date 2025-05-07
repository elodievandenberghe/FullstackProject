using BookingSite.Domains.DatabaseContext;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class CityDAO : IDAO<City, int>
{
    private readonly Context _dbContext;

    public CityDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(City entity)
    {
        try
        {
            await _dbContext.Cities.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(City entity)
    {
        try
        {
            _dbContext.Cities.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<City?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Cities.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<City>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.Cities.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(City entity)
    {
        try
        {
            _dbContext.Cities.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}