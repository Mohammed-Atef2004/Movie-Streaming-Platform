using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Controllers
{
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
            var series = _seriesService.GetAllSeries();
            return View(series);
        }
        public IActionResult Details(int id)
        {
            var series = _seriesService.GetSeriesById(id); // service layer
            if (series == null)
            {
                return NotFound();
            }
            return View(series);
        }
        [HttpPost]
        public IActionResult Download(int id)
        {
            var series = _seriesService.GetSeriesById(id);
            if (series == null || string.IsNullOrEmpty(series.ImageUrl))
            {
                return NotFound("No File Exist");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", series.ImageUrl);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = Path.GetFileName(filePath);
            series.DownloadCount = +1;
            _seriesService.UpdateSeries(series,series.Id);
            return File(fileBytes, "application/octet-stream", fileName);

        }
        [HttpPost]
        public IActionResult View(int id)
        {
            var series = _seriesService.GetSeriesById(id);
            if (series == null || string.IsNullOrEmpty(series.ImageUrl))
            {
                return NotFound("No Series Exist");
            }
            series.ViewCount = +1;
            _seriesService.UpdateSeries(series,series.Id);
            return View(series);
        }
    }
}
