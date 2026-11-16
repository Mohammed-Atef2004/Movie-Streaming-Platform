using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // Bug fix: these actions used to render views that did not exist in the
        // Customer area (Series/MovieDetails/SeriesDetails -> 500) or duplicated
        // the public browse pages with weaker features. They now delegate to the
        // real, searchable/paged pages under /Movies and /Series.

        public IActionResult Movies()
        {
            return RedirectToAction("Index", "Movies", new { area = "" });
        }

        public IActionResult Series()
        {
            return RedirectToAction("Index", "Series", new { area = "" });
        }

        public IActionResult MovieDetails(int id)
        {
            return RedirectToAction("Details", "Movies", new { area = "", id });
        }

        public IActionResult SeriesDetails(int id)
        {
            return RedirectToAction("Details", "Series", new { area = "", id });
        }

        public IActionResult MoviesSortedByViews()
        {
            return RedirectToAction("Index", "Movies", new { area = "", sortBy = "views" });
        }

        public IActionResult SeriesSortedByViews()
        {
            return RedirectToAction("Index", "Series", new { area = "", sortBy = "views" });
        }

        public IActionResult MoviesSortedByDownloads()
        {
            return RedirectToAction("Index", "Movies", new { area = "", sortBy = "downloads" });
        }

        public IActionResult SeriesSortedByDownloads()
        {
            return RedirectToAction("Index", "Series", new { area = "", sortBy = "downloads" });
        }
    }
}
