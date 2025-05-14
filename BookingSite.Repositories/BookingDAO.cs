using BookingSite.Domains.DatabaseContext;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class BookingDAO : IBookingDAO
{
    private readonly Context _dbContext;

    public BookingDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Booking entity)
    {
        try
        {
            await _dbContext.Bookings.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Booking entity)
    {
        try
        {
            _dbContext.Bookings.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Booking?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Bookings
                .Include(b => b.Tickets)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Booking>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.Bookings
                .Include(b => b.Tickets)
                .Include(b => b.User)
                .Where(b => b.Tickets.Count > 0)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Booking entity)
    {
        try
        {
            _dbContext.Bookings.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId)
    {
        try
        {
            return await _dbContext.Bookings
                .Include(b => b.Tickets)
                .ThenInclude(t => t.Flight)
                .Include(b => b.Tickets)
                .Include(b => b.Tickets)
                .ThenInclude(t => t.Meal)
                .Include(b => b.Tickets)
                .ThenInclude(t => t.Flight.Route.FromAirport)
                .Include(b => b.Tickets)
                .ThenInclude(t => t.Flight.Route.ToAirport)

                .Where(b => b.UserId == userId)
                .Where(b => b.Tickets.Count > 0)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBookingsByUserIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<String>?> GetCityLattitudeLongitudeOfLastBookedTicketsAsync(string userId)
    {
        try
        {
            return await _dbContext.Bookings
                .Where(b => b.UserId == userId)
                .Where(b => b.Tickets.Count > 0)
                .OrderBy(b => b.Id)
                .Select(b => b.Tickets
                    .Select(t => t.Flight.Route.ToAirport.City.LatLong).Distinct().ToList())
                .LastOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetFirstBookingByUserIdAsync: {ex.Message}");
            throw;
        }
    }
}
