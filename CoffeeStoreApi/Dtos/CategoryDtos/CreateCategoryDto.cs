using System;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Dtos
{
	public class CreateCategoryDto
	{
        public string Name { set; get; } = string.Empty;

        public string? ImageUrl { get; set; }

    }
}

