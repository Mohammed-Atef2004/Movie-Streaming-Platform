using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace BLL.ViewModels
{
    public class SeriesVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Maximum length for the title is 100 character")]
        public string Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [ValidateNever]
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
        public bool? IsFree { get; set; } = true;
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [ValidateNever] // Bug #1 Fix: Added ValidateNever so ModelState doesn't fail on missing Category object
        public Category Category { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}