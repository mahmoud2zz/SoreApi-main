using System.IO;
using CoffeeStoreApi.Common;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Helpers;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Repositories.Interfaces;
using CoffeeStoreApi.Services.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeStoreApi.Services.Products
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _Repository;

        private readonly IFileService _fileService;

        public ProductServices(ApplicationDbContext dbContext, IProductRepository repository, IFileService fileService)
        {

            _Repository = repository;
            _fileService = fileService;
        }

        public async Task<Response<ProudctResponseDto>> CreateProduct(ProductDto productDto)
        {

            var imageBytes = await _fileService.GetFileBytesAsync(productDto.file!);
            Product product = new Product() { Name = productDto.Name, Price = productDto.Price, Description = productDto.Description, Stock = productDto.Stock, IsActive = productDto.IsActive, Image = imageBytes!, CategoryId = productDto.categoryId };
            await _Repository.CreateProudct(product);
            return ResponseBuilder.Success<ProudctResponseDto>(
                "Operation Successful",
                MapToDto(product)
                

            );
        }

        public async Task<Response<List<ProudctResponseDto>>> GetProudcts()
        {
            var products =  await _Repository.GetProducts();
            return ResponseBuilder.Success<List<ProudctResponseDto>>("Operation Successful",  products.Select(p => MapToDto(p)).ToList());
        }

        public async Task<Response<ProudctResponseDto?>> RemoveProudct(int id)
        {
            var product = await _Repository.RemoveProduct(id);

            if (product == null)
                return ResponseBuilder.Failure<ProudctResponseDto?>("Proudct Not Found", null);

            return ResponseBuilder.Success<ProudctResponseDto?>("Operation Successful", MapToDto(product));

        }

        public async Task<Response<ProudctResponseDto?>> UpdateProudct(ProductDto productDto, int id)
        {
            var product = await _Repository.GetProductById(id);
            if (product == null)
                return ResponseBuilder.Failure<ProudctResponseDto?>("Proudct Not Found", null);

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.Stock = productDto.Stock;
            product.IsActive = productDto.IsActive;
            product.CategoryId = productDto.categoryId;

            if (productDto.file != null)
            {
                product.Image = await _fileService
                    .GetFileBytesAsync(productDto.file);
            }
            await _Repository.Update(product);
            return ResponseBuilder.Success<ProudctResponseDto?>("Operation Successful", MapToDto(product));

        }

        public async Task<Response<ProudctResponseDto?>> GetProudct(int id)
        {
            var product = await _Repository.GetProductById(id);

            if (product == null)
                return ResponseBuilder.Failure<ProudctResponseDto?>("Proudct Not Found", null);

            return ResponseBuilder.Success<ProudctResponseDto?>("Operation Successful", MapToDto(product));
        }


        private ProudctResponseDto MapToDto(Product product)
        {
            return new ProudctResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Stock = product.Stock,
                IsActive = product.IsActive,
                Category = product.Categor?.Name??"",
                ImageBase64 = product.Image != null
                    ? Convert.ToBase64String(product.Image)
                    : null
            };
        }
    }



}

