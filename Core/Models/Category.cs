using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Category : CommonData
    {
        public int Id { get; private set; }
        [Required]
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public ICollection<Movie> Movies { get; set; }

        public void Create(string name, string? description = null, string? creator = null)
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = creator;
        }
        public void Update(string name, string? description = null, string? updater = null)
        {
            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updater;
        }
        public void Delete(string name)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = name;
        }
    }
}
