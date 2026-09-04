using System;
using CoffeeStoreApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoffeeStoreApi.Dtos
{
    public class ProudctResponseDto
    {

        public int Id { set; get; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
        public string Category { set; get; } = string.Empty;
        public string? ImageBase64 { get; set; }
     
    }
}

