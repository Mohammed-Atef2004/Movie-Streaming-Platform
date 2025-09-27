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
    public class MovieVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage="Maximum length for the title is 100 character")]
        public string Title { get; set; }
        [Required]
        public string? Description { get; set; }
        public int Views { get; set; } = 0;
        public int Downloads { get; set; } = 0;
        public IFormFile? Image { get; set; } 
        public string? ImageUrl { get; set; }
        public bool? IsFree { get; set; } = true;
        public IEnumerable<SelectListItem> ?CategoryList { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string DisplayImageUrl => string.IsNullOrEmpty(ImageUrl) ? "placeholder.png" : ImageUrl;
    }
}
