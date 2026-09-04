using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CoffeeStoreApi.Enums;

namespace CoffeeStoreApi.Models
{
    public class Product
    {
        public int Id { get; set; }  

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int Stock { get; set; }

        public bool IsActive { get; set; } = true;

        public byte[]? Image { set; get; }

        

        [JsonIgnore]
        public Category? Categor { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public int CategoryId { set; get; }

        public ICollection<OrderItem> OrderItems { get; }
            = new List<OrderItem>();
    }
}
