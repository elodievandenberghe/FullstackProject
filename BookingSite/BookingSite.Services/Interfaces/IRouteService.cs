using BookingSite.Domains.Models;

namespace BookingSite.Services.Interfaces;

public interface IRouteService : IService<Route, int>
{
    Task<IEnumerable<Route>?> GetByFromAirportIdToAirportId(int fromAirportId, int toAirportId);
}