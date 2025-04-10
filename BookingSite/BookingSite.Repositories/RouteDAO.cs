using BookingSite.Domains.Context;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class RouteDAO : IDAO<Route, int>
{
    private readonly Context _dbContext;

    public RouteDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Route entity)
    {
        try
        {
            await _dbContext.Routes.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Route entity)
    {
        try
        {
            _dbContext.Routes.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Route?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Routes.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Route>> GetAllAsync()
    {
        try
        {
            return await _dbContext.Routes
                .Include(b => b.FromAirport)
                .Include(b => b.ToAirport)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Route entity)
    {
        try
        {
            _dbContext.Routes.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}