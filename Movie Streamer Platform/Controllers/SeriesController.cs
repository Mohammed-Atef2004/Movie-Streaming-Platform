using Application.Services.Abstraction;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Movie_Streamer_Platform.Controllers
{
    public class SeriesController : Controller
    {
        private readonly ISeriesService _seriesService;
        private readonly ICategoryService _categoryService;
        private readonly IEpisodeService _episodeService;
        private readonly IUserSeriesService _userSeriesService;

        public SeriesController(
            ISeriesService seriesService,
            ICategoryService categoryService,
            IEpisodeService episodeService,
            IUserSeriesService userSeriesService)
        {
            _seriesService = seriesService;
            _categoryService = categoryService;
            _episodeService = episodeService;
            _userSeriesService = userSeriesService;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId, string? sortBy, int page = 1)
        {
            const int pageSize = 12;

            var (series, totalCount) = await _seriesService.SearchSeriesAsync(search, categoryId, sortBy, page, pageSize);

            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.CategoryId = categoryId;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

            return View(series);
        }

        public async Task<IActionResult> Details(int id)
        {
            var series = await _seriesService.GetSeriesByIdAsync(id);
            if (series == null)
            {
                return NotFound();
            }

            ViewBag.Episodes = await _episodeService.GetEpisodesBySeriesAsync(id);
            ViewBag.IsFavorite = false;
            ViewBag.IsInWatchList = false;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.IsFavorite = await _userSeriesService.IsFavoriteAsync(userId, id);
                ViewBag.IsInWatchList = await _userSeriesService.IsInWatchListAsync(userId, id);
            }

            return View(series);
        }

        /// <summary>
        /// Watch page for a series: renders a player for the selected episode
        /// (or the first one) plus the full episode list.
        /// </summary>
        [Authorize]
        public async Task<IActionResult> View(int id, int? episodeId)
        {
            var series = await _seriesService.GetSeriesByIdAsync(id);
            if (series == null)
            {
                return NotFound();
            }

            var episodes = (await _episodeService.GetEpisodesBySeriesAsync(id)).ToList();

            var current = episodes.FirstOrDefault(e => e.Id == episodeId) ?? episodes.FirstOrDefault();
            if (current == null || string.IsNullOrEmpty(current.VideoUrl))
            {
                TempData["Error"] = "This series has no episodes available yet.";
                return RedirectToAction(nameof(Details), new { id });
            }

            if (!current.Id.Equals(episodeId ?? 0) || episodeId == null)
            {
                await _episodeService.IncrementEpisodeViewsAsync(current.Id);
            }

            ViewBag.Episodes = episodes;
            ViewBag.CurrentEpisode = current;

            return View(series);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(int seriesId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _userSeriesService.IsFavoriteAsync(userId, seriesId))
                await _userSeriesService.RemoveFromFavoritesAsync(userId, seriesId);
            else
                await _userSeriesService.AddToFavoritesAsync(userId, seriesId);

            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
                return Ok(new { success = true });

            return RedirectToAction(nameof(Details), new { id = seriesId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleWatchList(int seriesId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _userSeriesService.IsInWatchListAsync(userId, seriesId))
                await _userSeriesService.RemoveFromWatchListAsync(userId, seriesId);
            else
                await _userSeriesService.AddToWatchListAsync(userId, seriesId);

            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
                return Ok(new { success = true });

            return RedirectToAction(nameof(Details), new { id = seriesId });
        }

        /// <summary>
        /// Bug fix: the old implementation "downloaded" the poster image with
        /// application/octet-stream and had the '= +1' counter bug. Now the
        /// selected episode's actual video is downloaded and the counter is
        /// incremented atomically.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Download(int id, int? episodeId)
        {
            var episodes = (await _episodeService.GetEpisodesBySeriesAsync(id)).ToList();

            var episode = episodes.FirstOrDefault(e => e.Id == episodeId) ?? episodes.FirstOrDefault();
            if (episode == null || string.IsNullOrEmpty(episode.VideoUrl))
            {
                return NotFound("No video available for this series yet.");
            }

            await _episodeService.IncrementEpisodeDownloadsAsync(episode.Id);

            if (Uri.IsWellFormedUriString(episode.VideoUrl, UriKind.Absolute) &&
                (episode.VideoUrl.StartsWith("http://") || episode.VideoUrl.StartsWith("https://")))
            {
                return Redirect(episode.VideoUrl);
            }

            var fileName = Path.GetFileName(episode.VideoUrl);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos", fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/octet-stream", fileName);
        }
    }
}
