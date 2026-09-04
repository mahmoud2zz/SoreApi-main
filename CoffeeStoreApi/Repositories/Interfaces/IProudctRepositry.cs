using System;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Repositories.Interfaces
{
	public interface IProductRepository
    {
		public Task<Product> CreateProudct(Product product);

        public Task<List<Product>> GetProducts();

        public Task<Product?> GetProductById(int id);

        public Task<Product> Update(Product product);

        public Task<Product?> RemoveProduct(int id);

    }
}

