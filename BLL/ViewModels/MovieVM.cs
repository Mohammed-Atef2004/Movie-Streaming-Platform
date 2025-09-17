using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public int? ViewCount { get; set; }
        public int? DownloadCount { get;  set; }
        public double Price { get; set; }
    }
}
