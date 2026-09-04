using System;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Services.Payments
{
	public interface IPaymentService
	{
        Task<string> CreatePaymentIntent(Order order);

    }
}

