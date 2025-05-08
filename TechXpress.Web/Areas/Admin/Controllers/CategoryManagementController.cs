using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Services.CategoriesService;

namespace TechXpress.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryManagementController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryManagementController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Admin/CategoryManagement
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllWithProductsAsync();
            foreach (var item in categories)
            {
                item.ProductCount = item.Products.Count;

            }
            return View(categories);
        }

        // GET: Admin/CategoryManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetByIdWithProductsAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // GET: Admin/CategoryManagement/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ParentCategories = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");

            return View();
        }

        // POST: Admin/CategoryManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            //remove id from ModelState
            ModelState.Remove("id");
            if (ModelState.IsValid)
            {
                await _categoryService.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/CategoryManagement/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            ViewBag.ParentCategories = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");

            return View(category);
        }

        // POST: Admin/CategoryManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCategory = await _categoryService.GetByIdAsync(id);

                    if (existingCategory == null)
                        return NotFound();

                    // Manually update fields instead of re-attaching the entity
                    existingCategory.Name = model.Name;
                    existingCategory.MetaTitle = model.MetaTitle;
                    existingCategory.MetaDescription = model.MetaDescription;
                    existingCategory.MetaKeywords = model.MetaKeywords;
                    existingCategory.ParentId = model.ParentId;
                    existingCategory.Slug = model.Slug;
                    existingCategory.ImageUrl = model.ImageUrl;
                    existingCategory.Description = model.Description;
                    existingCategory.DisplayOrder = model.DisplayOrder;
                    existingCategory.IsActive = model.IsActive;
                    existingCategory.ModifiedDate = DateTime.UtcNow;
                    existingCategory.Level = model.Level;
                    existingCategory.CreatedDate = model.CreatedDate;
                    existingCategory.ProductCount = model.ProductCount;
                    existingCategory.ParentCategory=_categoryService.GetByIdAsync(model.ParentId).Result;

                    // Update the category


                    await _categoryService.UpdateAsync(existingCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    // Log the error (not shown here)
                    throw;
                }
            }

            // If model state is invalid, refill dropdown
            ViewBag.ParentCategories = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name", model.ParentId);
            return View(model);
        }


        // GET: Admin/CategoryManagement/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Admin/CategoryManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
