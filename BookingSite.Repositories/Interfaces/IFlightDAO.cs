using BookingSite.Domains.Models;

namespace BookingSite.Repositories.Interfaces;

public interface IFlightDAO : IDAO<Flight, int>
{
    Task<IEnumerable<Flight>> FindByFromAndToAirportIdAsync(int toId, int fromId);
    Task<IEnumerable<Flight>> GetAllFilteredAsync(int? fromAirportId, int? toAirportId, DateTime? departureDate);
}
