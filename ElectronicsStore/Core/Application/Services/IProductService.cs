using ElectronicsStore.Core.Application.DTOs;
using System.Collections.Generic;

namespace ElectronicsStore.Core.Application.Services
{
    public interface IProductService
    {
        ProductDto GetProductById(int id);
        IEnumerable<ProductDto> GetAllProducts();
        ProductDto CreateProduct(ProductDto productDto);
        ProductDto UpdateProduct(ProductDto productDto);
        void DeleteProduct(int id);
    }
}
