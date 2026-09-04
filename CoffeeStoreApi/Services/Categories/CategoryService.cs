using System;
using CoffeeStoreApi.Common;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Helpers;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Repositories.Interfaces;

namespace CoffeeStoreApi.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<CategoryResponseDto>> CreateCategory(CreateCategoryDto dto)
        {

            Category category = await _categoryRepository.CreateCategory(new Category() { Name = dto.Name, ImageUrl = dto.ImageUrl });

            return ResponseBuilder.Success("Successfully opration", new CategoryResponseDto() { Name = category.Name, ImageUrl = dto.ImageUrl, Products = new List<Product>() });
        }

        public async Task<Response<List<CategoryResponseDto>>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategories();
            List<CategoryResponseDto> result = categories
       .Select(c => new CategoryResponseDto
       {
           Name = c.Name,
           ImageUrl = c.ImageUrl,
           Products = c.Products
       })
         .ToList();

            return ResponseBuilder.Success( "Successfully opration",result);

        }

        public async Task<Response<CategoryResponseDto?>> RemoveCategories(int id)
        {
            Category? category = await _categoryRepository.RemoveCategory(id);

            if (category == null)
                ResponseBuilder.Failure< CategoryResponseDto?>("Not Found Category",null);

            return ResponseBuilder.Success(
        "Category removed successfully",
        new CategoryResponseDto
        {
            Name = category.Name,
            ImageUrl = category.ImageUrl,
            Products = category.Products
        });

        }
    }
}

