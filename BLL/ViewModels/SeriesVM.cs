using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class SeriesVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage="Maximum length for the title is 100 character")]
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
        public Category Category { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }

    }
}
