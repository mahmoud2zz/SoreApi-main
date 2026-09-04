using System;
using CoffeeStoreApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoffeeStoreApi.Dtos
{
	public class ProductDto
	{
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int Stock { get; set; }

        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public IFormFile? file { set; get; } = null;


        [Required]
        public int categoryId { set; get; }
           


    }
}

