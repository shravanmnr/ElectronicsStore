using ElectronicsStore.Models;

namespace ElectronicsStore.Core.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }

        public static ProductDto FromEntity(Product product)
        {
            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name
            };
        }

        public static Product ToEntity(ProductDto dto)
        {
            if (dto == null)
                return null;

            return new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId
            };
        }
    }
}
