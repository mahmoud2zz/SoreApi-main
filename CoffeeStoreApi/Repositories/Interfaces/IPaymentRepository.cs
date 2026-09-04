using System;
using CoffeeStoreApi.Domain;

namespace CoffeeStoreApi.Repositories.Interfaces
{
	public interface IPaymentRepository
	{
        Task CreatePayment(Payment payment);
    }
}

