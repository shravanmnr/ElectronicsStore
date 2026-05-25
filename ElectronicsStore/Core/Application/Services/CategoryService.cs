using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using ElectronicsStore.Core.Application.DTOs;
using ElectronicsStore.Core.Application.Validators;
using ElectronicsStore.Core.Data.Repositories;

namespace ElectronicsStore.Core.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<CategoryDto> _validator;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _validator = new CategoryValidator();
        }

        public CategoryDto GetCategoryById(int id)
        {
            var category = _categoryRepository.GetById(id);
            return CategoryDto.FromEntity(category);
        }

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            var categories = _categoryRepository.GetAll();
            return categories.Select(c => CategoryDto.FromEntity(c)).ToList();
        }

        public CategoryDto CreateCategory(CategoryDto categoryDto)
        {
            if (categoryDto == null)
                throw new ArgumentNullException(nameof(categoryDto));

            var validationResult = _validator.Validate(categoryDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var category = CategoryDto.ToEntity(categoryDto);
            _categoryRepository.Add(category);
            _categoryRepository.SaveChanges();

            return CategoryDto.FromEntity(category);
        }

        public CategoryDto UpdateCategory(CategoryDto categoryDto)
        {
            if (categoryDto == null)
                throw new ArgumentNullException(nameof(categoryDto));

            var validationResult = _validator.Validate(categoryDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var category = _categoryRepository.GetById(categoryDto.Id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found.");

            category.Name = categoryDto.Name;

            _categoryRepository.Update(category);
            _categoryRepository.SaveChanges();

            return CategoryDto.FromEntity(category);
        }

        public void DeleteCategory(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            _categoryRepository.Remove(category);
            _categoryRepository.SaveChanges();
        }
    }
}
