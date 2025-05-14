using BookingSite.Domains.Models;

namespace BookingSite.Services.Interfaces;

public interface IFlightService : IService<Flight, int>
{
    Task<IEnumerable<Flight>> FindByFromAndToAirportIdAsync(int fromId, int toId);
}