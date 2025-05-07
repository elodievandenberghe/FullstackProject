using BookingSite.Domains.DatabaseContext;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class PlaneDAO : IPlaneDAO
{
    private readonly Context _dbContext;

    public PlaneDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Plane entity)
    {
        try
        {
            await _dbContext.Planes.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Plane entity)
    {
        try
        {
            _dbContext.Planes.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Plane?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Planes
                .Include(p => p.Flights)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Plane>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.Planes
                .Include(p => p.Flights)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Plane>> GetPlanesWithAvailableSeatsAsync(int minimumSeats)
    {
        try
        {
            // This query finds planes where at least one flight has minimumSeats or more seats available
            return await _dbContext.Planes
                .Include(p => p.Flights)
                .ThenInclude(f => f.Tickets.Where(t => t.IsCancelled != true))
                .Where(p => p.Flights.Any(f => f.Tickets.Count(t => t.IsCancelled != true) <= p.Capacity - minimumSeats))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetPlanesWithAvailableSeatsAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Plane entity)
    {
        try
        {
            _dbContext.Planes.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}
