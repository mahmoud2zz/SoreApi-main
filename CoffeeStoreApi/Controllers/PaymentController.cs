using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeStoreApi.Authorization;
using CoffeeStoreApi.Services.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoffeeStoreApi.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {

        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("create-payment")]
        [Authorize(Policy = Permissions.CreatePayment)]
        public async Task<IActionResult> CreatePayment([FromForm] int orderId)
        {
            var result = await _paymentService.CreatePayment(orderId);

            if (!result.Success)
              return  NotFound(result);

            return Ok(result);
        }


       
       

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body)
                .ReadToEndAsync();

            var stripeSignature = Request.Headers["Stripe-Signature"].ToString();

            var result = await _paymentService.HandleWebhook(
                json,
                stripeSignature
            );

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }




        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

