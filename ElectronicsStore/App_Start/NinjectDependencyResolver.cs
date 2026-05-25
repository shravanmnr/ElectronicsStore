using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;

namespace ElectronicsStore.App_Start
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        public object GetService(Type serviceType)
        {
            // For controllers, propagate the real exception so we can see what's wrong
            if (typeof(System.Web.Mvc.IController).IsAssignableFrom(serviceType))
                return _kernel.Get(serviceType);

            try
            {
                return _kernel.TryGet(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _kernel.GetAll(serviceType);
            }
            catch
            {
                return new List<object>();
            }
        }
    }
}
