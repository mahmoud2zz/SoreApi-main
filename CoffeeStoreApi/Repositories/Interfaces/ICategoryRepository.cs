using System;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Repositories.Interfaces
{
	public interface ICategoryRepository
	{
		Task<Category> CreateCategory(Category category);

		Task<List<Category>> GetCategories();

        Task<Category?> RemoveCategory(int id);

    }
}

