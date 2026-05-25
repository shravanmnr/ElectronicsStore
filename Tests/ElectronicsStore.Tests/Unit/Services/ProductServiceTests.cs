using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentValidation;
using ElectronicsStore.Core.Application.DTOs;
using ElectronicsStore.Core.Application.Services;
using ElectronicsStore.Core.Data.Repositories;
using ElectronicsStore.Models;

namespace ElectronicsStore.Tests.Unit.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly IProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        [Fact]
        public void GetAllProducts_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.00m, Stock = 5 },
                new Product { Id = 2, Name = "Product 2", Price = 20.00m, Stock = 10 }
            };
            _mockProductRepository.Setup(r => r.GetAllWithCategory()).Returns(products);

            // Act
            var result = _productService.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockProductRepository.Verify(r => r.GetAllWithCategory(), Times.Once);
        }

        [Fact]
        public void GetProductById_ValidId_ReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Product 1", Price = 10.00m, Stock = 5 };
            _mockProductRepository.Setup(r => r.GetProductWithCategory(productId)).Returns(product);

            // Act
            var result = _productService.GetProductById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Product 1", result.Name);
            Assert.Equal(10.00m, result.Price);
        }

        [Fact]
        public void GetProductById_InvalidId_ReturnsNull()
        {
            // Arrange
            var productId = 999;
            _mockProductRepository.Setup(r => r.GetProductWithCategory(productId)).Returns((Product)null);

            // Act
            var result = _productService.GetProductById(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateProduct_ValidInput_ReturnsSuccessfullyCreatedProduct()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "New Product",
                Description = "Test Description",
                Price = 25.00m,
                Stock = 10,
                ImageUrl = "http://example.com/image.jpg"
            };

            Product capturedProduct = null;
            _mockProductRepository.Setup(r => r.Add(It.IsAny<Product>()))
                .Callback<Product>(p => capturedProduct = p);

            // Act
            var result = _productService.CreateProduct(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Product", result.Name);
            _mockProductRepository.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
            _mockProductRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CreateProduct_InvalidPrice_ThrowsValidationException()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "New Product",
                Price = -10.00m,
                Stock = 10
            };

            // Act & Assert
            Assert.Throws<ValidationException>(() => _productService.CreateProduct(productDto));
        }

        [Fact]
        public void CreateProduct_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _productService.CreateProduct(null));
        }

        [Fact]
        public void UpdateProduct_ValidInput_UpdatesSuccessfully()
        {
            // Arrange
            var productId = 1;
            var existingProduct = new Product { Id = productId, Name = "Old Name", Price = 10.00m, Stock = 5 };
            var updateDto = new ProductDto
            {
                Id = productId,
                Name = "Updated Name",
                Price = 15.00m,
                Stock = 8
            };

            _mockProductRepository.Setup(r => r.GetById(productId)).Returns(existingProduct);

            // Act
            var result = _productService.UpdateProduct(updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.Name);
            _mockProductRepository.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
            _mockProductRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateProduct_NonExistentId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = 999;
            var updateDto = new ProductDto
            {
                Id = productId,
                Name = "Updated Name",
                Price = 15.00m,
                Stock = 8
            };

            _mockProductRepository.Setup(r => r.GetById(productId)).Returns((Product)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _productService.UpdateProduct(updateDto));
        }

        [Fact]
        public void DeleteProduct_ValidId_DeletesSuccessfully()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Product to Delete" };
            _mockProductRepository.Setup(r => r.GetById(productId)).Returns(product);

            // Act
            _productService.DeleteProduct(productId);

            // Assert
            _mockProductRepository.Verify(r => r.Remove(product), Times.Once);
            _mockProductRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteProduct_NonExistentId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = 999;
            _mockProductRepository.Setup(r => r.GetById(productId)).Returns((Product)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _productService.DeleteProduct(productId));
        }
    }
}
