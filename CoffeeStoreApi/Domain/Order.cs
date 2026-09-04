
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CoffeeStoreApi.Domain;
using CoffeeStoreApi.Enums;
namespace CoffeeStoreApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { set; get; }

        public string? UserId { set; get; } = string.Empty;

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public List<OrderItem> Items { get; } = new List<OrderItem>();

       public  DeliveryInformation DeliveryInformation { set; get; } = null!;

        public Payment? Payment { get; set; }
    }
}
