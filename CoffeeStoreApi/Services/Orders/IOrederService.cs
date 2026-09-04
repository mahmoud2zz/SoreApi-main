using CoffeeStoreApi.Common;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Enums;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Services.Orders
{
	public interface IOrederService
	{

		public Task<Response<OrderResponseDto>> CreateOrder(CreateOrderDto dto,string userId);

		public Task<Response<OrderResponseDto?>> RemoveOrder(int id);

		public Task<Response<List<OrderResponseDto>>> GetOrders();


        public Task<Response<OrderResponseDto?>> UpdateOrder(int id, OrderStatus status);


    }
}

