using BookingSite.Domains.DatabaseContext;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class FlightDAO : IFlightDAO
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
            return await _dbContext.Flights
                .Include(f => f.Plane)
                .Include(f => f.Route)
                .Include(f => f.Route.ToAirport)
                .Include(f => f.Route.FromAirport)
                .Include(f => f.Route.RouteSegments)
                .ThenInclude(rs => rs.Airport)
                .Include(f => f.Tickets)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Flight>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.Flights
                .Include(f => f.Route.ToAirport)
                .Include(f => f.Route.FromAirport)
                .Include(f => f.Route.RouteSegments)
                .ThenInclude(rs => rs.Airport)
                .Include(f => f.Plane)
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

    public async Task<IEnumerable<Flight>> FindByFromAndToAirportIdAsync(int fromId, int toId)
    {
        try
        {
            return await _dbContext.Flights.Include(f => f.Plane)
                .Include(f => f.Route)
                .Include(f => f.Route.ToAirport)
                .Include(f => f.Route.FromAirport)
                .Include(f => f.Route.RouteSegments)
                .ThenInclude(rs => rs.Airport)
                .Include(f => f.Tickets)
                .Where(f => f.Route.FromAirportId == fromId && f.Route.ToAirportId == toId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Flight>> GetAllFilteredAsync(int? fromAirportId, int? toAirportId, DateTime? departureDate)
    {
        try
        {
            var query = _dbContext.Flights
                .Include(f => f.Route.ToAirport)
                .Include(f => f.Route.FromAirport)
                .Include(f => f.Route.RouteSegments)
                .ThenInclude(rs => rs.Airport)
                .Include(f => f.Plane)
                .AsQueryable();

            if (fromAirportId.HasValue)
            {
                query = query.Where(f => f.Route.FromAirportId == fromAirportId.Value);
            }

            if (toAirportId.HasValue)
            {
                query = query.Where(f => f.Route.ToAirportId == toAirportId.Value);
            }

            if (departureDate.HasValue)
            {
                var date = DateOnly.FromDateTime(departureDate.Value);
                query = query.Where(f => f.Date == date);
            }

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllFilteredAsync: {ex.Message}");
            throw;
        }
    }
}