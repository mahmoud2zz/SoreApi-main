using System;
using CoffeeStoreApi.Common;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Services.Products
{
	public interface IProductServices
	{
		public  Task<Response<ProudctResponseDto>> CreateProduct(ProductDto productDto);

        public Task<Response<ProudctResponseDto?>> UpdateProudct(ProductDto productDto, int id);

		public Task<Response<List<ProudctResponseDto>>> GetProudcts();

		public Task<Response<ProudctResponseDto?>> RemoveProudct(int id);

        public Task<Response<ProudctResponseDto?>> GetProudct(int id);



    }
}

