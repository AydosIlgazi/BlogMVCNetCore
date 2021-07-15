using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }
        public string Content { get; set; }
        public string HomePageContent { get; set; }
        public DateTime PostDate { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
