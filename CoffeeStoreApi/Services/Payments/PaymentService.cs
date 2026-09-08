using CoffeeStoreApi.Common;
using CoffeeStoreApi.Domain;
using CoffeeStoreApi.Dtos.PaymentDtos;
using CoffeeStoreApi.Enums;
using CoffeeStoreApi.Helpers;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Repositories.Interfaces;
using Stripe;

namespace CoffeeStoreApi.Services.Payments
{
    public class PaymentService : IPaymentService
    {

        private readonly IPaymentRepository _paymentRepository;

        private readonly IOrderRepositroy _orderRepositroy;

        private readonly IConfiguration _configuration;


        public PaymentService(IPaymentRepository paymentRepository, IOrderRepositroy orderRepositroy, IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _orderRepositroy = orderRepositroy;
            _configuration = configuration;
        }


        public async Task<Response<PaymentResponseDto?>> CreatePayment(int orderId)
        {
            var order = await _orderRepositroy.GetOrderByID(orderId);

            if (order == null)
                return ResponseBuilder.Failure<PaymentResponseDto?>("Order not found", null);

            var amount = order.Items.Sum(i => i.Quantity * i.UnitPrice);


            // information  PaymentIntent
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string>
                  {
                      "card"
                  }
            };

            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent paymentIntent = await service.CreateAsync(options);

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = (decimal)amount,
                PaymentIntentId = paymentIntent.Id,
                Status = PaymentStatus.Pending
            };

            await _paymentRepository.CreatePayment(payment);

            return ResponseBuilder.Success<PaymentResponseDto?>("payment created successfully", new PaymentResponseDto() { Amount = payment.Amount, OrderId = payment.OrderId, PaymentIntentId = paymentIntent.Id, ClientSecret = paymentIntent.ClientSecret });

        }

        public async Task<Response<string>> HandleWebhook(  string json,string stripeSignature)
        {
            var webhookSecret = _configuration["Stripe:WebhookSecret"];

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignature,
                webhookSecret
            );

            if (stripeEvent.Type != "payment_intent.succeeded" &&
                stripeEvent.Type != "payment_intent.payment_failed")
            {
                return ResponseBuilder.Failure<string>(
                    "Webhook ignored",
                    "Event type not handled"
                );
            }

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            if (paymentIntent == null)
                return ResponseBuilder.Failure<string>(
                    "PaymentIntent not found"
                );

            var payment = await _paymentRepository.GetPayment(paymentIntent.Id);

            if (payment == null)
                return ResponseBuilder.Failure<string>(
                    "Payment not found"
                );

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                payment.Status = PaymentStatus.Succeeded;

                await _paymentRepository.UpdatePayment(payment);

                var order = await _orderRepositroy.GetOrderByID(payment.OrderId);

                if (order == null)
                    return ResponseBuilder.Failure<string>(
                        "Order not found"
                    );

                order.OrderStatus = OrderStatus.Paid;

                await _orderRepositroy.UpdateOrder(order);
            }
            else if (stripeEvent.Type == "payment_intent.payment_failed")
            {
                payment.Status = PaymentStatus.Failed;

                await _paymentRepository.UpdatePayment(payment);
            }

            return ResponseBuilder.Success<string>(
                "Webhook handled successfully",
                "Payment processed successfully"
            );
        }

    }

}

