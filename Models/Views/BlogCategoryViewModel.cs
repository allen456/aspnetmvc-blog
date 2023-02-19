using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace aspnetmvc_blog.Models.Views
{
    public class BlogCategoryViewModel
    {
        public string CategoryName { get; set; } = string.Empty;
        public List<BlogDataViewModel> NewestBlog { get; set; } = new List<BlogDataViewModel>();
        public List<BlogDataCategoryViewModel> ArchiveList { get; set; } = new List<BlogDataCategoryViewModel>();
    }
}
 