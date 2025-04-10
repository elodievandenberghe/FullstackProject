using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class MealchoicesService : IService<MealChoice, int>
{
    private IDAO<MealChoice, int> _mealChoiceDAO;

    public MealchoicesService(IDAO<MealChoice, int> mealchoiceDAO) 
    {
        _mealChoiceDAO = mealchoiceDAO;
    }

    public async Task AddAsync(MealChoice entity)
    {
        await _mealChoiceDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(MealChoice entity)
    {
        await _mealChoiceDAO.DeleteAsync(entity);
    }

    public async Task<MealChoice?> FindByIdAsync(int Id)
    {
        return await _mealChoiceDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<MealChoice>?> GetAllAsync()
    {
        return await _mealChoiceDAO.GetAllAsync();
    }

    public async Task UpdateAsync(MealChoice entity)
    {
        await _mealChoiceDAO.UpdateAsync(entity);
    }
}