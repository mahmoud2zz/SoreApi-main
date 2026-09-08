using System;
namespace CoffeeStoreApi.Dtos.PaymentDtos
{
	public class PaymentResponseDto
	{
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}

