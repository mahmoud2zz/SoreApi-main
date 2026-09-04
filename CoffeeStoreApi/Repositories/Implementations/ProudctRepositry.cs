using System;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeStoreApi.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> CreateProudct(Product product)
        {
           await _dbContext.AddAsync(product);
           await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> GetProductById(int id)
        {
            Product? product = await _dbContext.Products.FindAsync(id);
            return product!=null ?product : null;
        }


        public async Task<List<Product>> GetProducts()
        {
            return await _dbContext.Products
                .Include(p => p.Categor)
                .ToListAsync();
        }

        public async Task<Product?> RemoveProduct(int id)
        {
            Product? product = await _dbContext.Products.FindAsync(id);

            if (product is null)
                return null;

            _dbContext.Products.Remove(product);
             await _dbContext.SaveChangesAsync();
            return product;

        }

        public async Task<Product> Update(Product product)
        {
            _dbContext.Products.Update(product);
           await _dbContext.SaveChangesAsync();
            return product;
        }
    }
}

