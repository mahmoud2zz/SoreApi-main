using System;
using CoffeeStoreApi.Dtos.DeliveryInformationDtos;

namespace CoffeeStoreApi.Dtos

{
	public class CreateOrderDto
	{
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public DeliveryInformationDto deliveryInformation { set; get; }=null!;

    }
}

