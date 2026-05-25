using ElectronicsStore.Models;
using System.Collections.Generic;

namespace ElectronicsStore.Core.Data.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Product GetProductWithCategory(int id);
        IEnumerable<Product> GetAllWithCategory();
    }
}
