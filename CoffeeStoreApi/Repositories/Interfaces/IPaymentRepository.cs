using System;
using CoffeeStoreApi.Domain;
using Stripe;

namespace CoffeeStoreApi.Repositories.Interfaces
{
	public interface IPaymentRepository
	{
        Task CreatePayment(Payment payment);

        Task<Payment?> GetPayment(string paymentIntent);

        Task UpdatePayment(Payment payment);
    }
}

