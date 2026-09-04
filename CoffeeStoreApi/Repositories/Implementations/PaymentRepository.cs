using CoffeeStoreApi.Domain;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Repositories.Interfaces;

namespace CoffeeStoreApi.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PaymentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreatePayment(Payment payment)
        {
            await _dbContext.Payments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();
        }
    }
}