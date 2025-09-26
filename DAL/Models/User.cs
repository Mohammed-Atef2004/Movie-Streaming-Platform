using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User : CommonData
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        
        public void Create(string name, string password, string? email = null, string? creator = null)
        {
            Name = name;
           
            Password = password;
            Email = email;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = creator;
            IsDeleted = false;
        }
        public void Update(string name, string password,string? email = null, string? updater = null)
        {
            Name = name;
            Email = email;
            Password = password;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updater;
        }
        public void Delete(string? deletedBy = null)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy; 
        }


    }


}
