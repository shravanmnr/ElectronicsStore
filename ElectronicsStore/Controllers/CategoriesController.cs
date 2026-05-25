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
    [RoutePrefix("categories")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        // GET /categories  → 200 OK
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            var categories = _categoryService.GetAllCategories()
                                             .Select(dto => CategoryDto.ToEntity(dto))
                                             .ToList();
            return OkView(categories);
        }

        // GET /categories/{id}  → 200 OK | 400 Bad Request | 404 Not Found
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest("Category ID is required.");

            var categoryDto = _categoryService.GetCategoryById(id.Value);
            if (categoryDto == null)
                return HttpNotFound($"Category with ID {id} was not found.");

            return OkView(CategoryDto.ToEntity(categoryDto));
        }

        // GET /categories/create  → 200 OK
        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            return OkView();
        }

        // POST /categories/create  → 302 Redirect | 200 OK (validation) | 500 Internal Server Error
        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _categoryService.CreateCategory(new CategoryDto { Name = category.Name });
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
                // 500 — unexpected server fault
                ModelState.AddModelError("", "An unexpected error occurred while creating the category.");
                return ServerErrorView(category);
            }

            return OkView(category);
        }

        // GET /categories/{id}/edit  → 200 OK | 400 Bad Request | 404 Not Found
        [HttpGet]
        [Route("{id:int}/edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest("Category ID is required.");

            var categoryDto = _categoryService.GetCategoryById(id.Value);
            if (categoryDto == null)
                return HttpNotFound($"Category with ID {id} was not found.");

            return OkView(CategoryDto.ToEntity(categoryDto));
        }

        // POST /categories/{id}/edit  → 302 Redirect | 200 OK (validation) | 404 Not Found | 500 Internal Server Error
        [HttpPost]
        [Route("{id:int}/edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _categoryService.UpdateCategory(new CategoryDto { Id = category.Id, Name = category.Name });
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
                // 404 — category was deleted between GET and POST
                return HttpNotFound($"Category with ID {category.Id} was not found.");
            }
            catch (Exception)
            {
                // 500 — unexpected server fault
                ModelState.AddModelError("", "An unexpected error occurred while updating the category.");
                return ServerErrorView(category);
            }

            return OkView(category);
        }

        // GET /categories/{id}/delete  → 200 OK | 400 Bad Request | 404 Not Found
        [HttpGet]
        [Route("{id:int}/delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest("Category ID is required.");

            var categoryDto = _categoryService.GetCategoryById(id.Value);
            if (categoryDto == null)
                return HttpNotFound($"Category with ID {id} was not found.");

            return OkView(CategoryDto.ToEntity(categoryDto));
        }

        // POST /categories/{id}/delete  → 302 Redirect | 404 Not Found | 500 Internal Server Error
        [HttpPost]
        [Route("{id:int}/delete")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _categoryService.DeleteCategory(id);
                return RedirectToAction("Index"); // 302
            }
            catch (KeyNotFoundException)
            {
                return HttpNotFound($"Category with ID {id} was not found.");
            }
            catch (Exception)
            {
                return InternalServerError("An unexpected error occurred while deleting the category.");
            }
        }
    }
}
