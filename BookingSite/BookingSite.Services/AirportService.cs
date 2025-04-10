using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class AirportService : IService<Airport, int>
{
    //private BeerDAO beerDAO;
    //public BeerService()
    //{
    //    // later via DI
    //    beerDAO = new BeerDAO();
    //}

    private IDAO<Airport, int> _airportDAO;

    public AirportService(IDAO<Airport, int> airportDAO) // DI
    {
        _airportDAO = airportDAO;
    }

    public async Task AddAsync(Airport entity)
    {
         await _airportDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(Airport entity)
    {
        await _airportDAO.DeleteAsync(entity);
    }

    public async Task<Airport?> FindByIdAsync(int Id)
    {
        return await _airportDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<Airport>?> GetAllAsync()
    {
        return await _airportDAO.GetAllAsync();
    }

    public async Task UpdateAsync(Airport entity)
    {
        await _airportDAO.UpdateAsync(entity);
    }
}

