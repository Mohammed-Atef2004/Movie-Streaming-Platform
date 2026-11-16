using BLL.Services.Abstraction;
using BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    /// <summary>
    /// NEW: full admin CRUD for episodes. The entity, repository and service
    /// already existed but were dead code (never registered in DI, no UI).
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EpisodeController : Controller
    {
        private readonly IEpisodeService _episodeService;
        private readonly ISeriesService _seriesService;

        public EpisodeController(IEpisodeService episodeService, ISeriesService seriesService)
        {
            _episodeService = episodeService;
            _seriesService = seriesService;
        }

        public async Task<IActionResult> Index(int? seriesId)
        {
            var episodes = seriesId.HasValue
                ? await _episodeService.GetEpisodesBySeriesAsync(seriesId.Value)
                : await _episodeService.GetAllEpisodesAsync();

            ViewBag.SeriesList = await BuildSeriesListAsync(seriesId);
            ViewBag.FilterSeriesId = seriesId;
            return View(episodes);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? seriesId)
        {
            var vm = new EpisodeVM
            {
                SeriesId = seriesId ?? 0,
                SeriesList = await BuildSeriesListAsync(seriesId)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EpisodeVM episodeVM)
        {
            if (ModelState.IsValid)
            {
                var (isSuccess, message) = await _episodeService.CreateEpisodeAsync(episodeVM, actor: User.Identity?.Name);
                if (isSuccess)
                {
                    TempData["Message"] = message;
                    return RedirectToAction(nameof(Index), new { seriesId = episodeVM.SeriesId });
                }

                ModelState.AddModelError("", message);
            }

            episodeVM.SeriesList = await BuildSeriesListAsync(episodeVM.SeriesId);
            return View(episodeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var episode = await _episodeService.GetEpisodeByIdAsync(id);
            if (episode == null)
                return NotFound();

            episode.SeriesList = await BuildSeriesListAsync(episode.SeriesId);
            return View(episode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EpisodeVM episodeVM)
        {
            if (ModelState.IsValid)
            {
                var (isSuccess, message) = await _episodeService.UpdateEpisodeAsync(episodeVM, episodeVM.Id, actor: User.Identity?.Name);
                if (isSuccess)
                {
                    TempData["Message"] = message;
                    return RedirectToAction(nameof(Index), new { seriesId = episodeVM.SeriesId });
                }

                ModelState.AddModelError("", message);
            }

            episodeVM.SeriesList = await BuildSeriesListAsync(episodeVM.SeriesId);
            return View(episodeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int seriesId)
        {
            var (_, message) = await _episodeService.DeleteEpisodeAsync(id, actor: User.Identity?.Name);
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index), new { seriesId });
        }

        private async Task<IEnumerable<SelectListItem>> BuildSeriesListAsync(int? selectedId = null)
        {
            var series = await _seriesService.GetAllSeriesAsync();
            return series.Select(s => new SelectListItem
            {
                Text = s.Title,
                Value = s.Id.ToString(),
                Selected = selectedId.HasValue && s.Id == selectedId.Value
            });
        }
    }
}
