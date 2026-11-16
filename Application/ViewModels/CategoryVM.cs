using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Maximum length for the name is 100 characters")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
