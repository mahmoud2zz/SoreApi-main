using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeStoreApi.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category> CreateCategory(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _dbContext.Categories.Include(c=>c.Products)
                
                .ToListAsync();
        }

        public async Task<Category?> RemoveCategory(int id)
        {
            Category? category = await _dbContext.Categories
                .FindAsync(id);
            if (category is null)
                return null;
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }
    }
}