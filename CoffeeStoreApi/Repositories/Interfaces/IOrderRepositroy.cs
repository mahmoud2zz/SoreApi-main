using System;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Repositories.Interfaces
{
	public interface IOrderRepositroy
	{

		Task<Order> CreateOrder(Order order);

        Task UpdateOrder(Order order);

        Task<Order> RmoveOrder(Order order);

		Task<Order?> GetOrderByID(int id);

		Task<List<Order>> GetOrders();

    }
}

