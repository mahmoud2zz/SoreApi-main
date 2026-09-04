using System;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Repositories.Implementations
{
    public class DeliveryInformationRepositroy : IDeliveryInformationRepositroy
    {
        private readonly ApplicationDbContext _dbContext;

        public DeliveryInformationRepositroy(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateDeliveryInformation(DeliveryInformation deliveryInformation)
        {
            await  _dbContext.Deliveries.AddAsync(deliveryInformation);
            await _dbContext.SaveChangesAsync();
        }

       
    }
}

