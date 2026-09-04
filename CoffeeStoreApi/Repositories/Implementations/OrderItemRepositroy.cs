using System;
using CoffeeStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeStoreApi.Repositories.Implementations
{
    public class OrderItemRepositroy : IOrderItemRepositroy
    {

        private readonly ApplicationDbContext _dbContext;

        public OrderItemRepositroy(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateOrderItem(OrderItem orderItem)
        {
            await _dbContext.orderItems.AddAsync(orderItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveOrderItem(OrderItem orderItem)
        {
             _dbContext.orderItems.Remove(orderItem);
              await _dbContext.SaveChangesAsync();

        }
    }
}



