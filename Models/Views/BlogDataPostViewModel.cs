using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models.Views
{
    public class BlogDataPostViewModel
    {
        public string Category { get; set; } = string.Empty;
        public bool Feature { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public DateTime BlogDate { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
 