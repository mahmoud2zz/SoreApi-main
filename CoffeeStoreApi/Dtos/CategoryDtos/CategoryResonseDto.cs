using System;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Dtos
{
	public class CategoryResponseDto
    {
        public string Name { set; get; } = string.Empty;

        public string? ImageUrl { get; set; }

        public ICollection<Product>? Products = new List<Product> { };
    }
}

