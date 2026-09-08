using CoffeeStoreApi.Domain;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Payment?> GetPayment(string paymentIntent)
        {
         return await  _dbContext.Payments.FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntent);
        }

        public async Task UpdatePayment(Payment payment)
        {
            _dbContext.Payments.Update(payment);
             await _dbContext.SaveChangesAsync();
        }
    }
}