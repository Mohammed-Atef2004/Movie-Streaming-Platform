using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class BookmarkVM
    {
        public int Id { get; set; }         
        public string UserId { get; set; }    
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public string ImageUrl { get; set; }

    }
}
