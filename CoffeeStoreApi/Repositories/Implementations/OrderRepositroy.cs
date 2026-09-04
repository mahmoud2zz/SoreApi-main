using System;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeStoreApi.Repositories.Implementations
{
    public class OrderRepositroy : IOrderRepositroy
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepositroy(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            await  _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
            
        }

        public async Task<Order?> GetOrderByID(int id)=> await _dbContext.Orders.Include(o => o.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(o => o.Id == id);




        public async Task<List<Order>> GetOrders() => await _dbContext.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToListAsync();
       
        

        public async Task<Order> RmoveOrder(Order order)
        {
             _dbContext.Orders.Remove(order);
            await  _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrder(Order order)
        {
            _dbContext.Update(order);
           await _dbContext.SaveChangesAsync();
        }
    }
}

