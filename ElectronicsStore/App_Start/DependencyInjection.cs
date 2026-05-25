using Ninject;
using ElectronicsStore.Core.Application.Services;
using ElectronicsStore.Core.Data.Repositories;
using ElectronicsStore.Models;

namespace ElectronicsStore.App_Start
{
    public class DependencyInjection
    {
        public static void RegisterServices(IKernel kernel)
        {
            // Register DbContext
            kernel.Bind<ApplicationDbContext>().ToSelf();

            // Register Repositories
            kernel.Bind<IProductRepository>().To<ProductRepository>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();

            // Register Services
            kernel.Bind<IProductService>().To<ProductService>();
            kernel.Bind<ICategoryService>().To<CategoryService>();
        }
    }
}
