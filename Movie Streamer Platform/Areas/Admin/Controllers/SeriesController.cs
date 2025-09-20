using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Series_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SeriesController : Controller
    {
        private readonly ISeriesService _seriesService;
        public SeriesController(ISeriesService seriesService)
        {
            _seriesService = seriesService;
        }
        public IActionResult GetAll()
        {
            var series=_seriesService.GetAllSeries();
            return View(series);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
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
                    ViewBag.Success = message;
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.Error = message;
                    return View(seriesVM);
                }
            }
            return View(seriesVM);

        }
        public IActionResult Delete(int Id)
        {
            _seriesService.DeleteSeries(Id);
            return RedirectToAction("GetAll");
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var series=_seriesService.GetSeriesById(Id);
            if (series != null)
            {
                return View(series);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(SeriesVM seriesVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                seriesVM.Image = file;
                var (isSuccess, message) = _seriesService.UpdateSeries(seriesVM, seriesVM.Id);
                if (isSuccess)
                {
                    ViewBag.Success = message;
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.Error = message;
                    return View(seriesVM);
                }
            }
            return View(seriesVM);
        }
       
    }
  
}
