using BookingSite.Domains.Context;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class RouteSegmentsDAO : IDAO<RouteSegment, int>
{
    private readonly Context _dbContext;

    public RouteSegmentsDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(RouteSegment entity)
    {
        try
        {
            await _dbContext.RouteSegments.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(RouteSegment entity)
    {
        try
        {
            _dbContext.RouteSegments.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<RouteSegment?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.RouteSegments.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<RouteSegment>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.RouteSegments
                .Include(b => b.Route)
                .Include(b => b.Airport)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(RouteSegment entity)
    {
        try
        {
            _dbContext.RouteSegments.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}