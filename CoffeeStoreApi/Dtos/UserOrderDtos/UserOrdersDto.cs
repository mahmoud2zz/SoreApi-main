using System;
namespace CoffeeStoreApi.Dtos
{
	public class UserOrdersDto
	{
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<OrderResponseDto> Orders { get; set; }
    }
}

