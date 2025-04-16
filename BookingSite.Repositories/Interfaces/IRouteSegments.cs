using BookingSite.Domains.Models;

namespace BookingSite.Repositories.Interfaces;

public interface IRouteSegments : IDAO<RouteSegment, int>
{
    Task<IEnumerable<RouteSegment>?> GetByRouteId(int routeId);
}