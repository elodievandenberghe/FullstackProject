using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class RouteService : IRouteService
{
    private IRouteDAO _routeDAO;

    public RouteService(IRouteDAO routeDao) 
    {
        _routeDAO = routeDao;
    }

    public async Task AddAsync(Route entity)
    {
        await _routeDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(Route entity)
    {
        await _routeDAO.DeleteAsync(entity);
    }

    public async Task<Route?> FindByIdAsync(int Id)
    {
        return await _routeDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<Route>?> GetAllAsync()
    {
        return await _routeDAO.GetAllAsync();
    }

    public async Task UpdateAsync(Route entity)
    {
        await _routeDAO.UpdateAsync(entity);
    }

    public async Task<IEnumerable<Route>?> GetByFromAirportIdToAirportId(int fromAirportId, int toAirportId)
    {
        return await _routeDAO.GetByFromAirportIdToAirportId(fromAirportId, toAirportId); 
    }
}