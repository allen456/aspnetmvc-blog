using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace aspnetmvc_blog.Models.Views
{
    public class BlogDataCategoryViewModel
    {
        public int year { get; set; }
        public int month { get; set; }
        public string name { get; set; } = string.Empty;
    }
}