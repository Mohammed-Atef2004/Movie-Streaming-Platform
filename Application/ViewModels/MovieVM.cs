using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{
    public class MovieVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Maximum length for the title is 100 characters")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000, ErrorMessage = "Maximum length for the description is 2000 characters")]
        public string? Description { get; set; }

        public int Views { get; set; } = 0;
        public int Downloads { get; set; } = 0;

        [ValidateNever]
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }

        public bool IsFree { get; set; } = true;

        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please choose a category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category? Category { get; set; }

        [MaxLength(500)]
        public string? TrailerUrl { get; set; }

        [MaxLength(500)]
        public string? MovieUrl { get; set; }

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        public int Year { get; set; } = DateTime.Now.Year;

        [Display(Name = "Rating")]
        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
        public double Rating { get; set; } = 5;

        public string DisplayImageUrl => string.IsNullOrEmpty(ImageUrl) ? "placeholder.png" : ImageUrl;
    }
}
