using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class PlaneService : IPlaneService
{
    private readonly IPlaneDAO _planeDAO;

    public PlaneService(IPlaneDAO planeDAO)
    {
        _planeDAO = planeDAO;
    }

    public async Task AddAsync(Plane entity)
    {
        await _planeDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(Plane entity)
    {
        await _planeDAO.DeleteAsync(entity);
    }

    public async Task<Plane?> FindByIdAsync(int Id)
    {
        return await _planeDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<Plane>?> GetAllAsync()
    {
        return await _planeDAO.GetAllAsync();
    }

    public async Task<IEnumerable<Plane>> GetPlanesWithAvailableSeatsAsync(int minimumSeats)
    {
        return await _planeDAO.GetPlanesWithAvailableSeatsAsync(minimumSeats);
    }

    public async Task UpdateAsync(Plane entity)
    {
        await _planeDAO.UpdateAsync(entity);
    }
}
