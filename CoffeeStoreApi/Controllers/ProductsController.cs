using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CoffeeStoreApi.Authorization;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Helpers;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoffeeStoreApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {

        private readonly IProductServices _productServices;

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        // GET: api/values
        [HttpGet("GetProducts")]
        [Authorize(Policy = Permissions.GetProducts)]
        public async Task<IActionResult> GetProducts()
        {
        
            return Ok(await _productServices.GetProudcts());
        }

        [Authorize(Policy = Permissions.GetProductById)]
        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProudctById( int id)
        {
            var respones=  await _productServices.GetProudct(id);

            if (!respones.Success)
                return NotFound(respones);

            return Ok(respones);
        }



        // POST api/values
        
        [HttpPost("CreateProudct")]
        [Authorize(Policy = Permissions.AddProduct)]
        public async Task<IActionResult> CreateProuct([FromForm] ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseBuilder.Failure<string?>("Data is Not Vaild", null));

            var response = await _productServices.CreateProduct(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize(Policy = Permissions.UpdateProduct)]
        public  async Task<IActionResult> UpdateProudct(int id, [FromForm] ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseBuilder.Failure<string?>("Data is Not Vaild", null));

            var response = await _productServices.UpdateProudct(dto,id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);

        }

        // DELETE api/values/5
       
        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.RemoveProduct)]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            return Ok(await _productServices.RemoveProudct(id));


        }
    }
}

