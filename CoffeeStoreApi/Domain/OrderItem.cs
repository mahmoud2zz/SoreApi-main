using System;
using System.Text.Json.Serialization;

namespace CoffeeStoreApi.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product? Product { set; get; }
        [JsonIgnore]
        public Order? Order { set; get; }
        public int Quantity { set; get; }
        public double UnitPrice { set; get; }

    }
}

