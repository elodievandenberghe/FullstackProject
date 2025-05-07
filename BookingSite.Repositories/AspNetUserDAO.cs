using BookingSite.Domains.DatabaseContext;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Repositories;

public class AspNetUserDao : IDAO<AspNetUser, string>
{
    private readonly Context _dbContext;

    public AspNetUserDao(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(AspNetUser entity)
    {
        try
        {
            await _dbContext.AspNetUsers.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(AspNetUser entity)
    {
        try
        {
            _dbContext.AspNetUsers.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<AspNetUser?> FindByIdAsync(string id)
    {
        try
        {
            return await _dbContext.AspNetUsers.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FindByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<AspNetUser>?> GetAllAsync()
    {
        try
        {
            return await _dbContext.AspNetUsers.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(AspNetUser entity)
    {
        try
        {
            _dbContext.AspNetUsers.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}