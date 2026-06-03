using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ElectronicsStore.Models;
using ElectronicsStore.Core.Application.Services;
using ElectronicsStore.Core.Application.DTOs;
using FluentValidation;

namespace ElectronicsStore.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        // GET /products  → 200 OK
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            var products = _productService.GetAllProducts().ToList();
            return OkView(products);
        }

        // GET /products/{id}  → 200 OK | 400 Bad Request | 404 Not Found
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest("Product ID is required.");

            var productDto = _productService.GetProductById(id.Value);
            if (productDto == null)
                return HttpNotFound($"Product with ID {id} was not found.");

            return OkView(ProductDto.ToEntity(productDto));
        }

        // GET /products/create  → 200 OK
        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            PopulateCategoryDropdown();
            return OkView();
        }

        // POST /products/create  → 302 Redirect | 200 OK (validation) | 500 Internal Server Error
        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,Stock,ImageUrl,CategoryId")] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _productService.CreateProduct(new ProductDto
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Stock = product.Stock,
                        ImageUrl = product.ImageUrl,
                        CategoryId = product.CategoryId
                    });
                    return RedirectToAction("Index"); // 302
                }
            }
            catch (ValidationException ex)
            {
                // 200 — form re-display with business rule violations
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred while creating the product.");
                PopulateCategoryDropdown(product.CategoryId);
                return ServerErrorView(product);
            }

            PopulateCategoryDropdown(product.CategoryId);
            return OkView(product);
        }

        // GET /products/{id}/edit  → 200 OK | 400 Bad Request | 404 Not Found
        [HttpGet]
        [Route("{id:int}/edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest("Product ID is required.");

            var productDto = _productService.GetProductById(id.Value);
            if (productDto == null)
                return HttpNotFound($"Product with ID {id} was not found.");

            var product = ProductDto.ToEntity(productDto);
            PopulateCategoryDropdown(product.CategoryId);
            return OkView(product);
        }

        // POST /products/{id}/edit  → 302 Redirect | 200 OK (validation) | 404 Not Found | 500 Internal Server Error
        [HttpPost]
        [Route("{id:int}/edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,Stock,ImageUrl,CategoryId")] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _productService.UpdateProduct(new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Stock = product.Stock,
                        ImageUrl = product.ImageUrl,
                        CategoryId = product.CategoryId
                    });
                    return RedirectToAction("Index"); // 302
                }
            }
            catch (ValidationException ex)
            {
                // 200 — form re-display with business rule violations
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            catch (KeyNotFoundException)
            {
                // 404 — product was deleted between GET and POST
                return HttpNotFound($"Product with ID {product.Id} was not found.");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred while updating the product.");
                PopulateCategoryDropdown(product.CategoryId);
                return ServerErrorView(product);
            }

            PopulateCategoryDropdown(product.CategoryId);
            return OkView(product);
        }

        // GET /products/{id}/delete  → 200 OK | 400 Bad Request | 404 Not Found
        [HttpGet]
        [Route("{id:int}/delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest("Product ID is required.");

            var productDto = _productService.GetProductById(id.Value);
            if (productDto == null)
                return HttpNotFound($"Product with ID {id} was not found.");

            return OkView(ProductDto.ToEntity(productDto));
        }

        // POST /products/{id}/delete  → 302 Redirect | 404 Not Found | 500 Internal Server Error
        [HttpPost]
        [Route("{id:int}/delete")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return RedirectToAction("Index"); // 302
            }
            catch (KeyNotFoundException)
            {
                return HttpNotFound($"Product with ID {id} was not found.");
            }
            catch (Exception)
            {
                return InternalServerError("An unexpected error occurred while deleting the product.");
            }
        }

        private void PopulateCategoryDropdown(int? selectedId = null)
        {
            var categories = _categoryService.GetAllCategories()
                                             .Select(c => CategoryDto.ToEntity(c))
                                             .ToList();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", selectedId);
        }
    }
}
