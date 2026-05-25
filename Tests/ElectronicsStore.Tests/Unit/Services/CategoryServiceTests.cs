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
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly ICategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockCategoryRepository.Object);
        }

        [Fact]
        public void GetAllCategories_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Computers" }
            };
            _mockCategoryRepository.Setup(r => r.GetAll()).Returns(categories);

            // Act
            var result = _categoryService.GetAllCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockCategoryRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetCategoryById_ValidId_ReturnsCategory()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Electronics" };
            _mockCategoryRepository.Setup(r => r.GetById(categoryId)).Returns(category);

            // Act
            var result = _categoryService.GetCategoryById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Electronics", result.Name);
        }

        [Fact]
        public void GetCategoryById_InvalidId_ReturnsNull()
        {
            // Arrange
            var categoryId = 999;
            _mockCategoryRepository.Setup(r => r.GetById(categoryId)).Returns((Category)null);

            // Act
            var result = _categoryService.GetCategoryById(categoryId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateCategory_ValidInput_ReturnsSuccessfullyCreatedCategory()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Name = "New Category"
            };

            Category capturedCategory = null;
            _mockCategoryRepository.Setup(r => r.Add(It.IsAny<Category>()))
                .Callback<Category>(c => capturedCategory = c);

            // Act
            var result = _categoryService.CreateCategory(categoryDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Category", result.Name);
            _mockCategoryRepository.Verify(r => r.Add(It.IsAny<Category>()), Times.Once);
            _mockCategoryRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CreateCategory_EmptyName_ThrowsValidationException()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Name = ""
            };

            // Act & Assert
            Assert.Throws<ValidationException>(() => _categoryService.CreateCategory(categoryDto));
        }

        [Fact]
        public void CreateCategory_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _categoryService.CreateCategory(null));
        }

        [Fact]
        public void UpdateCategory_ValidInput_UpdatesSuccessfully()
        {
            // Arrange
            var categoryId = 1;
            var existingCategory = new Category { Id = categoryId, Name = "Old Category" };
            var updateDto = new CategoryDto
            {
                Id = categoryId,
                Name = "Updated Category"
            };

            _mockCategoryRepository.Setup(r => r.GetById(categoryId)).Returns(existingCategory);

            // Act
            var result = _categoryService.UpdateCategory(updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Category", result.Name);
            _mockCategoryRepository.Verify(r => r.Update(It.IsAny<Category>()), Times.Once);
            _mockCategoryRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateCategory_NonExistentId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var categoryId = 999;
            var updateDto = new CategoryDto
            {
                Id = categoryId,
                Name = "Updated Category"
            };

            _mockCategoryRepository.Setup(r => r.GetById(categoryId)).Returns((Category)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _categoryService.UpdateCategory(updateDto));
        }

        [Fact]
        public void DeleteCategory_ValidId_DeletesSuccessfully()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Category to Delete" };
            _mockCategoryRepository.Setup(r => r.GetById(categoryId)).Returns(category);

            // Act
            _categoryService.DeleteCategory(categoryId);

            // Assert
            _mockCategoryRepository.Verify(r => r.Remove(category), Times.Once);
            _mockCategoryRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteCategory_NonExistentId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var categoryId = 999;
            _mockCategoryRepository.Setup(r => r.GetById(categoryId)).Returns((Category)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _categoryService.DeleteCategory(categoryId));
        }
    }
}
