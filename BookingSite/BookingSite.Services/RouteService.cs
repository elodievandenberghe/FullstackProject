using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class RouteService : IService<Route, int>
{
    private IDAO<Route, int> _routeDAO;

    public RouteService(IDAO<Route, int> routeDao) 
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

    public async Task<Route> FindByIdAsync(int Id)
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
}