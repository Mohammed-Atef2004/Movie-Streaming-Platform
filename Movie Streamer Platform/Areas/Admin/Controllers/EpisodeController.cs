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

    public class EpisodeController : Controller
    {
        private readonly IEpisodeService _episodeService;
        private readonly ISeriesService _seriesService;

        public EpisodeController(IEpisodeService episodeService, ISeriesService seriesService)
        {
            _episodeService = episodeService;
            _seriesService = seriesService;
        }

        public IActionResult Index()
        {
            var episode = _episodeService.GetAllEpisodes();
            return View(episode);
        }
        [HttpGet]
        public IActionResult Create()
        {
            EpisodeVM episodeVM = new EpisodeVM();
            episodeVM.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(episodeVM);
        }
        [HttpPost]
        public IActionResult Create(EpisodeVM episodeVM,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                episodeVM.Image= file;
                var (isSuccess, message) = _episodeService.CreateEpisode(episodeVM);
                if (isSuccess)
                {
                    episodeVM.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    episodeVM.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                    ModelState.AddModelError("", message);
                    return View(episodeVM);
                }
            }
            return View(episodeVM);

        }
        public IActionResult Delete(int Id)
        {
            _episodeService.DeleteEpisode(Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var episode=_episodeService.GetEpisodeById(Id);
            if (episode != null)
            {
                episode.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(episode);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(EpisodeVM episodeVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    episodeVM.Image = file;
                }
            }

            var (isSuccess, message) = _episodeService.UpdateEpisode(episodeVM, episodeVM.Id);
            if (isSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = message;
            }


            episodeVM.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = i.Id == episodeVM.CategoryId
            });

            return View(episodeVM);
        }

    }
  
}
