using BookingSite.Domains.Models;

namespace BookingSite.Services.Interfaces;


public interface IMealChoiceService : IService<MealChoice, int>
{
    Task<IEnumerable<MealChoice>?> GetByAirportId(int toAirportId);
}