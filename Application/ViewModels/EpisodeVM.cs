using Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{
    public class EpisodeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Maximum length for the title is 100 characters")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000, ErrorMessage = "Maximum length for the description is 2000 characters")]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? VideoUrl { get; set; }

        public int? ViewCount { get; set; }
        public int DownloadCount { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please choose a series")]
        public int SeriesId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? SeriesList { get; set; }

        [ValidateNever]
        public Series? Series { get; set; }
    }
}
