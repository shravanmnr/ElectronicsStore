using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ElectronicsStore.Models;

namespace ElectronicsStore.Core.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Product GetProductWithCategory(int id)
        {
            return _dbSet.Include("Category").FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAllWithCategory()
        {
            return _dbSet.Include("Category").ToList();
        }
    }
}
