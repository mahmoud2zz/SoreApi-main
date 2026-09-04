using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeStoreApi.Authorization;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Services.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoffeeStoreApi.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        // GET: api/values
        [HttpGet]
       
        public async Task<IActionResult> GetCategorie()
        {
            return Ok( await _categoryService.GetCategories());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("create-cateory")]
        [Authorize(Policy = Permissions.AddCategory)]
        public async Task<IActionResult> CreateCateory([FromForm] CreateCategoryDto dto)
        {
            var response =  await _categoryService.CreateCategory(dto);
            return Ok(response);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.RemoveCategory)]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            var response = await _categoryService.RemoveCategories(id);
            if (!response.Success)
                NotFound(response);
            return Ok(response);
        }
    }
}

