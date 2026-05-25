using ElectronicsStore.Core.Application.DTOs;
using System.Collections.Generic;

namespace ElectronicsStore.Core.Application.Services
{
    public interface ICategoryService
    {
        CategoryDto GetCategoryById(int id);
        IEnumerable<CategoryDto> GetAllCategories();
        CategoryDto CreateCategory(CategoryDto categoryDto);
        CategoryDto UpdateCategory(CategoryDto categoryDto);
        void DeleteCategory(int id);
    }
}
