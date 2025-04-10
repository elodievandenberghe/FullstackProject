using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class RouteSegmentsService : IService<RouteSegment, int>
{
    private IDAO<RouteSegment, int> _routeSegmentDAO;

    public RouteSegmentsService(IDAO<RouteSegment, int> routeSegmentDao) 
    {
        _routeSegmentDAO = routeSegmentDao;
    }

    public async Task AddAsync(RouteSegment entity)
    {
        await _routeSegmentDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(RouteSegment entity)
    {
        await _routeSegmentDAO.DeleteAsync(entity);
    }

    public async Task<RouteSegment?> FindByIdAsync(int Id)
    {
        return await _routeSegmentDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<RouteSegment>?> GetAllAsync()
    {
        return await _routeSegmentDAO.GetAllAsync();
    }

    public async Task UpdateAsync(RouteSegment entity)
    {
        await _routeSegmentDAO.UpdateAsync(entity);
    }
}