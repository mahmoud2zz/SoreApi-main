using System;
using CoffeeStoreApi.Common;
using CoffeeStoreApi.Dtos;

using CoffeeStoreApi.Repositories.Interfaces;

namespace CoffeeStoreApi.Services.Categories
{
	public interface ICategoryService
	{
        Task<Response<CategoryResponseDto>> CreateCategory(CreateCategoryDto dto);
        Task<Response<List<CategoryResponseDto>>> GetCategories();
        Task<Response<CategoryResponseDto?>> RemoveCategories(int id);
    }
}

