using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{
    public class SeriesVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Maximum length for the title is 100 characters")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000, ErrorMessage = "Maximum length for the description is 2000 characters")]
        public string? Description { get; set; }

        [ValidateNever]
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
        public bool? IsFree { get; set; } = true;

        [Range(1, int.MaxValue, ErrorMessage = "Please choose a category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category? Category { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}
