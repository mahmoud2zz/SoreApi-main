
using CoffeeStoreApi.Authorization;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Enums;
using CoffeeStoreApi.Helpers;
using CoffeeStoreApi.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoffeeStoreApi.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrederService _orederService;

        public OrdersController(IOrederService orederService)
        {
            _orederService = orederService;
        }


        // GET: api/values
        [Authorize(Policy = Permissions.GetOrders)]
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _orederService.GetOrders());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("create-order")]
        [Authorize(Policy = Permissions.CreateOrder)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseBuilder.Failure<string?>("Data is Not Vaild", null));

            var response = await _orederService.CreateOrder(dto, User.FindFirst("uid")?.Value);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


      

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize(Policy = Permissions.UpdateOrder)]
        public async Task<IActionResult> UpdateOrder(int id, OrderStatus status)
        {
          var response =await _orederService.UpdateOrder(id, status);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        // DELETE api/values/5
      
        [Authorize()]

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.RemoveOrder)]
        public async Task<IActionResult> RemoveOrder(int id)
        {
            return Ok( await _orederService.RemoveOrder(id));
        }
    }
}

