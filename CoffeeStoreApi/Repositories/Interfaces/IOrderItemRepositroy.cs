using System;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Repositories.Implementations
{
	public interface IOrderItemRepositroy
	{
		Task CreateOrderItem(OrderItem orderItem);

		Task RemoveOrderItem(OrderItem orderItem);

    }
}

