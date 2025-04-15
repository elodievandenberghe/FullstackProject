using BookingSite.Domains.Models;

namespace BookingSite.Repositories.Interfaces;

public interface IRouteDAO : IDAO<Route, int>
{
    Task<IEnumerable<Route>?> GetByFromAirportIdToAirportId(int fromAirportId, int toAirportId);
}