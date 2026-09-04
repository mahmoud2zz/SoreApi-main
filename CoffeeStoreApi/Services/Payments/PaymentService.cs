using System;
using CoffeeStoreApi.Models;
using Stripe;

namespace CoffeeStoreApi.Services.Payments
{
	public class PaymentService: IPaymentService
    {
        public async Task<string> CreatePaymentIntent(Order order)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.Items.Sum(i => i.Quantity * i.UnitPrice) * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string>
                {
                    "card"
                }
            };

            var service = new PaymentIntentService();

            var paymentIntent = await service.CreateAsync(options);

            return paymentIntent.Id;
        }

       
    }
}

