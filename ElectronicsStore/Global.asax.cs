using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using ElectronicsStore.App_Start;

namespace ElectronicsStore
{
	public class MvcApplication : System.Web.HttpApplication
	{
		private static IKernel _kernel;

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			_kernel = new StandardKernel();
			DependencyInjection.RegisterServices(_kernel);
			DependencyResolver.SetResolver(new NinjectDependencyResolver(_kernel));

			// Prevent duplicate "required" validation for non-nullable value types (decimal, int)
			System.Web.Mvc.DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
		}

		public void Application_End()
		{
			_kernel?.Dispose();
		}
	}
}
