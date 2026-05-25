using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;
using ElectronicsStore.Core.Data.Repositories;
using ElectronicsStore.Models;

namespace ElectronicsStore.Tests.Unit.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<DbSet<Product>> _mockDbSet;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _mockDbSet = new Mock<DbSet<Product>>();
            _mockContext.Setup(c => c.Set<Product>()).Returns(_mockDbSet.Object);
            _repository = new ProductRepository(_mockContext.Object);
        }

        [Fact]
        public void GetById_ValidId_ReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product" };
            _mockDbSet.Setup(d => d.Find(productId)).Returns(product);

            // Act
            var result = _repository.GetById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Product", result.Name);
            _mockDbSet.Verify(d => d.Find(productId), Times.Once);
        }

        [Fact]
        public void GetById_InvalidId_ReturnsNull()
        {
            // Arrange
            var productId = 999;
            _mockDbSet.Setup(d => d.Find(productId)).Returns((Product)null);

            // Act
            var result = _repository.GetById(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            }.AsQueryable();

            _mockDbSet.As<IQueryable<Product>>().Setup(d => d.Provider).Returns(products.Provider);
            _mockDbSet.As<IQueryable<Product>>().Setup(d => d.Expression).Returns(products.Expression);
            _mockDbSet.As<IQueryable<Product>>().Setup(d => d.ElementType).Returns(products.ElementType);
            _mockDbSet.As<IQueryable<Product>>().Setup(d => d.GetEnumerator()).Returns(products.GetEnumerator());

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Add_ValidProduct_AddsProductToDbSet()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "New Product" };

            // Act
            _repository.Add(product);

            // Assert
            _mockDbSet.Verify(d => d.Add(product), Times.Once);
        }

        [Fact]
        public void Add_NullProduct_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _repository.Add(null));
        }

        [Fact]
        public void Update_ValidProduct_UpdatesProductState()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated Product" };
            var mockEntry = new Mock<DbEntityEntry<Product>>();
            _mockContext.Setup(c => c.Entry(product)).Returns(mockEntry.Object);

            // Act
            _repository.Update(product);

            // Assert
            Assert.Equal(System.Data.Entity.EntityState.Modified, mockEntry.Object.State);
            _mockContext.Verify(c => c.Entry(product), Times.Once);
        }

        [Fact]
        public void Update_NullProduct_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _repository.Update(null));
        }

        [Fact]
        public void Remove_ValidProduct_RemovesProductFromDbSet()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product to Remove" };

            // Act
            _repository.Remove(product);

            // Assert
            _mockDbSet.Verify(d => d.Remove(product), Times.Once);
        }

        [Fact]
        public void Remove_NullProduct_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _repository.Remove(null));
        }

        [Fact]
        public void SaveChanges_CallsSaveChangesOnContext()
        {
            // Act
            _repository.SaveChanges();

            // Assert
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void GetProductWithCategory_ValidId_ReturnsProductWithCategory()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                CategoryId = 1,
                Category = new Category { Id = 1, Name = "Electronics" }
            };

            var products = new List<Product> { product }.AsQueryable();

            _mockDbSet.As<IQueryable<Product>>().Setup(d => d.Provider).Returns(products.Provider);
            _mockDbSet.As<IQueryable<Product>>().Setup(d => d.Expression).Returns(products.Expression);
            _mockDbSet.As<IQueryable<Product>>().Setup(d => d.ElementType).Returns(products.ElementType);
            _mockDbSet.As<IQueryable<Product>>().Setup(d => d.GetEnumerator()).Returns(products.GetEnumerator());

            // Act
            var result = _repository.GetProductWithCategory(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Product", result.Name);
            Assert.NotNull(result.Category);
            Assert.Equal("Electronics", result.Category.Name);
        }
    }
}
