using CoffeeStoreApi.Enums;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Domain
    {
        public class Payment
        {
            public int Id { get; set; }

            public int OrderId { get; set; }

            public decimal Amount { get; set; }

            public string PaymentIntentId { get; set; } = string.Empty;

            public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public Order Order { get; set; } = null!;
        }
    }


