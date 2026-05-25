using ElectronicsStore.Models;

namespace ElectronicsStore.Core.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static CategoryDto FromEntity(Category category)
        {
            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static Category ToEntity(CategoryDto dto)
        {
            if (dto == null)
                return null;

            return new Category
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
