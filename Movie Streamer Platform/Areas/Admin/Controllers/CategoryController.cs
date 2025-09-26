using BLL.Services.Abstraction;
using BLL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("Admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            var result=_categoryService.GetAllCategories();
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                var (isSuccess, message) = _categoryService.CreateCategory(categoryVM);
                if (isSuccess)
                {
                    ViewBag.Success = message;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = message;
                    return View(categoryVM);
                }
            }
            return View(categoryVM);
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var category = _categoryService.GetCategoryById(Id);
            if (category != null)
            {
                return View(category);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                var (isSuccess, message) = _categoryService.UpdateCategory(categoryVM);
                if (isSuccess)
                {
                    ViewBag.Success = message;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = message;
                    return View(categoryVM);
                }
            }
            return View(categoryVM);
        }
        public IActionResult Delete(int Id)
        {
            _categoryService.DeleteCategory(Id);
            return RedirectToAction("Index");
        }
    }
}
