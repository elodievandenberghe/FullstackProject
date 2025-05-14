using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class FlightService : IFlightService
{
    private IFlightDAO _flightDAO;

    public FlightService(IFlightDAO flightDAO) // DI
    {
        _flightDAO = flightDAO;
    }

    public async Task AddAsync(Flight entity)
    {
        await _flightDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(Flight entity)
    {
        await _flightDAO.DeleteAsync(entity);
    }

    public async Task<Flight?> FindByIdAsync(int Id)
    {
        return await _flightDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<Flight>?> GetAllAsync()
    {
        return await _flightDAO.GetAllAsync();
    }

    public async Task UpdateAsync(Flight entity)
    {
        await _flightDAO.UpdateAsync(entity);
    }

    public async Task<IEnumerable<Flight>> FindByFromAndToAirportIdAsync(int fromId, int toId)
    {
        return await _flightDAO.FindByFromAndToAirportIdAsync(fromId, toId);
    }
}