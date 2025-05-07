using BookingSite.Domains.DatabaseContext;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class TicketDAO : ITicketDAO
{
    private readonly Context _dbContext;

    public TicketDAO(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Ticket entity)
    {
        try
        {
            // Auto-assign seat if not already assigned and if flight has a plane
            if (!entity.SeatNumber.HasValue && entity.FlightId.HasValue)
            {
                entity.SeatNumber = await AssignSeatNumber(entity.FlightId.Value);
            }
            
            await _dbContext.Tickets.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Ticket entity)
    {
        try
        {
            _dbContext.Tickets.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Ticket?> FindByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Tickets
                .Include(t => t.Flight)
                .Include(t => t.Meal)
                .Include(t => t.Booking)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Ticket>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.Tickets
                .Include(t => t.Flight)
                .Include(t => t.Meal)
                .Include(t => t.Booking)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Ticket entity)
    {
        try
        {
            // Auto-assign seat if not already assigned and if flight has a plane
            if (!entity.SeatNumber.HasValue && entity.FlightId.HasValue)
            {
                entity.SeatNumber = await AssignSeatNumber(entity.FlightId.Value);
            }
            
            _dbContext.Tickets.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Ticket>?> GetByFlightIdAsync(int flightId)
    {
        try
        {
            return await _dbContext.Tickets
                .Where(t => t.FlightId == flightId)
                .Include(t => t.Flight)
                .Include(t => t.Meal)
                .Include(t => t.Booking)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetByFlightIdAsync: {ex.Message}");
            throw;
        }
    }

    private async Task<int?> AssignSeatNumber(int flightId)
    {
        // Get the flight with its plane
        var flight = await _dbContext.Flights
            .Include(f => f.Plane)
            .FirstOrDefaultAsync(f => f.Id == flightId);

        if (flight?.Plane == null)
        {
            return null; // No plane assigned to this flight
        }

        int capacity = flight.Plane.Capacity;

        // Get all seat numbers currently in use for this flight (for non-cancelled tickets)
        var usedSeats = await _dbContext.Tickets
            .Where(t => t.FlightId == flightId && t.IsCancelled != true && t.SeatNumber.HasValue)
            .Select(t => t.SeatNumber.Value)
            .ToListAsync();

        // Find the first available seat number
        for (int seatNum = 1; seatNum <= capacity; seatNum++)
        {
            if (!usedSeats.Contains(seatNum))
            {
                return seatNum;
            }
        }

        // No seats available
        return null;
    }
    
    public async Task<IEnumerable<Ticket>?> GetByFlightId(int flightId)
    {
        try
        {
            return await _dbContext.Tickets
                .Where(t => t.FlightId == flightId)
                .Include(t => t.Flight)
                .Include(t => t.Meal)
                .Include(t => t.Booking)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetByFlightId: {ex.Message}");
            throw;
        }
    }
}