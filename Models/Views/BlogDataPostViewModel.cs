using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models.Views
{
    public class BlogDataPostViewModel
    {
        public string Category { get; set; }
        public bool Feature { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public DateTime BlogDate { get; set; }
        public string Content { get; set; }
    }
}
 