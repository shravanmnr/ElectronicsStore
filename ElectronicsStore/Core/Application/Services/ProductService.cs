using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using ElectronicsStore.Core.Application.DTOs;
using ElectronicsStore.Core.Application.Validators;
using ElectronicsStore.Core.Data.Repositories;
using ElectronicsStore.Models;

namespace ElectronicsStore.Core.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<ProductDto> _validator;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _validator = new ProductValidator();
        }

        public ProductDto GetProductById(int id)
        {
            var product = _productRepository.GetProductWithCategory(id);
            return ProductDto.FromEntity(product);
        }

        public IEnumerable<ProductDto> GetAllProducts()
        {
            var products = _productRepository.GetAllWithCategory();
            return products.Select(p => ProductDto.FromEntity(p)).ToList();
        }

        public ProductDto CreateProduct(ProductDto productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto));

            var validationResult = _validator.Validate(productDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var product = ProductDto.ToEntity(productDto);
            _productRepository.Add(product);
            _productRepository.SaveChanges();

            return ProductDto.FromEntity(product);
        }

        public ProductDto UpdateProduct(ProductDto productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto));

            var validationResult = _validator.Validate(productDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var product = _productRepository.GetById(productDto.Id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productDto.Id} not found.");

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.ImageUrl = productDto.ImageUrl;
            product.CategoryId = productDto.CategoryId;

            _productRepository.Update(product);
            _productRepository.SaveChanges();

            return ProductDto.FromEntity(product);
        }

        public void DeleteProduct(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            _productRepository.Remove(product);
            _productRepository.SaveChanges();
        }
    }
}
