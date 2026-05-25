using ElectronicsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ElectronicsStore.Controllers
{
	public class HomeController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();
		public ActionResult Index(int? categoryId)
		{
			IQueryable<Product> products = db.Products.Include("Category");

			if (categoryId != null)
			{
				products = products.Where(p => p.CategoryId == categoryId);
			}

			ViewBag.Categories = db.Categories.ToList();
			ViewBag.SelectedCategory = categoryId;

			return View(products.ToList());
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}