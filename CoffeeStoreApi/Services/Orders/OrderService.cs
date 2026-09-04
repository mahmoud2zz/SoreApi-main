using CoffeeStoreApi.Common;
using CoffeeStoreApi.Domain;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Enums;

using CoffeeStoreApi.Helpers;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Repositories.Implementations;
using CoffeeStoreApi.Repositories.Interfaces;
using CoffeeStoreApi.Services.Payments;

namespace CoffeeStoreApi.Services.Orders
{
    public class OrderService : IOrederService
    {

        private readonly IOrderRepositroy _orderRepositroy;

        private readonly IProductRepository _productRepository;

        private IDeliveryInformationRepositroy _deliveryInformationRepositroy;

        private readonly IPaymentService _paymentService;

        public OrderService(IOrderRepositroy orderRepositroy,  IProductRepository productRepository, IDeliveryInformationRepositroy deliveryInformationRepositroy, IPaymentService paymentService)
        {
            _orderRepositroy = orderRepositroy;
            _productRepository = productRepository;
            _deliveryInformationRepositroy = deliveryInformationRepositroy;
            _paymentService = paymentService;


        }

        public async Task<Response<OrderResponseDto>> CreateOrder(CreateOrderDto dto, string userId)
        {
            dynamic TotalAmount = 0;

            if (dto.Items == null || !dto.Items.Any())
                return ResponseBuilder.Failure<OrderResponseDto>(
                    "Order must have at least one item", null);

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                UserId = userId
            };

            foreach (var item in dto.Items)
            {
                var product = await _productRepository.GetProductById(item.ProductId);

                if (product == null)
                    return ResponseBuilder.Failure<OrderResponseDto>(
                        $"Product with Id {item.ProductId} not found", null);

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });

                TotalAmount += product.Price * item.Quantity;
            }


            await _orderRepositroy.CreateOrder(order);
            var paymentIntentId = await _paymentService.CreatePaymentIntent(order);
            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = TotalAmount,
                PaymentIntentId = paymentIntentId,
                Status = PaymentStatus.Pending
            };

            DeliveryInformation deliveryInformation = new DeliveryInformation
            {
                FirstName = dto.deliveryInformation.FirstName,
                LastName = dto.deliveryInformation.LastName,
                Email = dto.deliveryInformation.Email,
                Phone = dto.deliveryInformation.Phone,
                Street = dto.deliveryInformation.Street,
                City = dto.deliveryInformation.City,
                State = dto.deliveryInformation.State,
                ZipCode = dto.deliveryInformation.ZipCode,
                Country = dto.deliveryInformation.Country,
                OrderId = order.Id
            };

           
            var orderResponseDto = new OrderResponseDto
            {
                OrderDate = order.OrderDate,
                TotalAmount = TotalAmount,
                Items = dto.Items,
                DeliveryInformation=dto.deliveryInformation
               
            };
           await _deliveryInformationRepositroy.CreateDeliveryInformation(deliveryInformation);

            return ResponseBuilder.Success(
                "Order created successfully",
                orderResponseDto);
        }

        public async Task<Response<List<OrderResponseDto>>> GetOrders()
        {
            var orders = await _orderRepositroy.GetOrders();

            return ResponseBuilder.Success<List<OrderResponseDto>>(
                "Orders retrieved successfully",
                orders.Select(o => MapToOrderDto(o)).ToList());
        }

        public async Task<Response<OrderResponseDto?>> RemoveOrder(int id)
        {

            var order = await _orderRepositroy.GetOrderByID(id);

            if (order == null)
            {
                return ResponseBuilder.Failure<OrderResponseDto?>("Order not found", null);
            }

            await _orderRepositroy.RmoveOrder(order);
            return ResponseBuilder.Success<OrderResponseDto?>(
    "Order deleted successfully",
    MapToOrderDto(order));


        }

        public  async Task<Response<OrderResponseDto?>> UpdateOrder(int id, OrderStatus status)
        {
            var order = await _orderRepositroy.GetOrderByID(id);

            if (order == null)
            {
                return ResponseBuilder.Failure<OrderResponseDto?>("Order not found", null);
            }
            order.OrderStatus = status;
           await _orderRepositroy.UpdateOrder(order);

            return ResponseBuilder.Success<OrderResponseDto?> ("Update Order", MapToOrderDto(order));
          
        }

        private OrderResponseDto MapToOrderDto(Order order)
        {
            return new OrderResponseDto
            {
                OrderDate = order.OrderDate,

                TotalAmount = order.Items.Sum(
                    i => i.Quantity * i.UnitPrice
                ),

                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product!.Name,
                    Quantity = i.Quantity,
                    UnitPrice = (decimal)i.UnitPrice
                }).ToList()
            };
        }



       }
    }








