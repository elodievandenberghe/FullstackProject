using BookingSite.Domains.Models;

namespace BookingSite.Services.Interfaces;

public interface IPlaneService : IService<Plane, int>
{
    Task<IEnumerable<Plane>> GetPlanesWithAvailableSeatsAsync(int minimumSeats);
}
