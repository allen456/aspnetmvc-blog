using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace aspnetmvc_blog.Models.Views
{
    public class BlogDetailedViewModel
    {
        public BlogDataViewModel BlogDisplay { get; set; }
        public List<BlogDataCategoryViewModel> ArchiveList { get; set; }
    }
}
