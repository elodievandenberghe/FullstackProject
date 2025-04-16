using BookingSite.Domains.Models;

namespace BookingSite.Repositories.Interfaces;

public interface IMealChoiceDAO : IDAO<MealChoice, int>
{
    Task<IEnumerable<MealChoice>?> GetByAirportId(int airportId);
}