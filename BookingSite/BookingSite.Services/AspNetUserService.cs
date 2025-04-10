using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class AspNetUserService : IService<AspNetUser, string>
{
    //private BeerDAO beerDAO;
    //public BeerService()
    //{
    //    // later via DI
    //    beerDAO = new BeerDAO();
    //}

    private IDAO<AspNetUser, string> _aspNetUserDAo;

    public AspNetUserService(IDAO<AspNetUser, string> aspNetUserDao) // DI
    {
        _aspNetUserDAo = aspNetUserDao;
    }

    public async Task AddAsync(AspNetUser entity)
    {
        await _aspNetUserDAo.AddAsync(entity);
    }

    public async Task DeleteAsync(AspNetUser entity)
    {
        await _aspNetUserDAo.DeleteAsync(entity);
    }

    public async Task<AspNetUser?> FindByIdAsync(string Id)
    {
        return await _aspNetUserDAo.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<AspNetUser>?> GetAllAsync()
    {
        return await _aspNetUserDAo.GetAllAsync();
    }

    public async Task UpdateAsync(AspNetUser entity)
    {
        await _aspNetUserDAo.UpdateAsync(entity);
    }
}