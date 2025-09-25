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
    }
}
