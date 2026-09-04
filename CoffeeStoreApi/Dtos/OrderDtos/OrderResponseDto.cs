using System;
using CoffeeStoreApi.Enums;
using System.ComponentModel.DataAnnotations;
using CoffeeStoreApi.Dtos.DeliveryInformationDtos;

namespace CoffeeStoreApi.Dtos
{
    public class OrderResponseDto
    {
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public DeliveryInformationDto DeliveryInformation { set; get; } = null!;

    }
}

