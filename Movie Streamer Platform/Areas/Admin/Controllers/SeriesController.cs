using AutoMapper;
using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using BLL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Series_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("Admin/[controller]")]
    [Authorize(Roles = "Admin")]

    public class SeriesController : Controller
    {
        private readonly ISeriesService _seriesService;
        private readonly ICategoryService _categoryService;

        public SeriesController(ISeriesService seriesService, ICategoryService categoryService)
        {
            _seriesService = seriesService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var series=_seriesService.GetAllSeries();
            return View(series);
        }
        [HttpGet]
        public IActionResult Create()
        {
            SeriesVM seriesVM = new SeriesVM();
            seriesVM.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(seriesVM);
        }
        [HttpPost]
        public IActionResult Create(SeriesVM seriesVM,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                seriesVM.Image= file;
                var (isSuccess, message) = _seriesService.CreateSeries(seriesVM);
                if (isSuccess)
                {
                    seriesVM.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    seriesVM.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                    ModelState.AddModelError("", message);
                    return View(seriesVM);
                }
            }
            return View(seriesVM);

        }
        public IActionResult Delete(int Id)
        {
            _seriesService.DeleteSeries(Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var series=_seriesService.GetSeriesById(Id);
            if (series != null)
            {
                series.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(series);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(SeriesVM seriesVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    seriesVM.Image = file;
                }
            }

            var (isSuccess, message) = _seriesService.UpdateSeries(seriesVM, seriesVM.Id);
            if (isSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = message;
            }


            seriesVM.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = i.Id == seriesVM.CategoryId
            });

            return View(seriesVM);
        }

    }
  
}
