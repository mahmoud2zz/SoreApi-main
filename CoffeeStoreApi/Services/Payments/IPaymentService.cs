using System;
using CoffeeStoreApi.Common;
using CoffeeStoreApi.Dtos.PaymentDtos;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Services.Payments
{
	public interface IPaymentService
	{
        Task <Response<PaymentResponseDto?>> CreatePayment(int orderId);

        Task<Response<string>>  HandleWebhook(string json, string stripeSignature);



    }
}

