using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Category : CommonData
    {
        public int Id { get; private set; }
        [Required]
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

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

        // Bug fix: the parameter used to be called "name" while it actually holds the deleting user's name.
        public void Delete(string deletedBy)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }
    }
}
