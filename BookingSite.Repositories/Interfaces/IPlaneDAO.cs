using BookingSite.Domains.Models;

namespace BookingSite.Repositories.Interfaces;

public interface IPlaneDAO : IDAO<Plane, int>
{
    Task<IEnumerable<Plane>> GetPlanesWithAvailableSeatsAsync(int minimumSeats);
}
