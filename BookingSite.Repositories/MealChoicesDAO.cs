using BookingSite.Domains.Context;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class MealChoicesDAO : IMealChoiceDAO
{
    private readonly Context _dbContext;

    public MealChoicesDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(MealChoice entity)
    {
        try
        {
            await _dbContext.MealChoices.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(MealChoice entity)
    {
        try
        {
            _dbContext.MealChoices.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<MealChoice?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.MealChoices.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<MealChoice>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.MealChoices
                .Include(b => b.Airport)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<MealChoice>?> GetByAirportId(int airportId)
    {
        return await _dbContext.MealChoices
            .Where(m => m.AirportId == null || m.AirportId == airportId)
            .Include(m => m.Airport)
            .ToListAsync();
       throw new NotImplementedException();
    }


    public async Task UpdateAsync(MealChoice entity)
    {
        try
        {
            _dbContext.MealChoices.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}