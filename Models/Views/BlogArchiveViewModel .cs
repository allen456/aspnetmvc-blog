﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace aspnetmvc_blog.Models.Views
{
    public class BlogArchiveViewModel
    {
        public string ArchiveName { get; set; }
        public List<BlogDataViewModel> NewestBlog { get; set; }
        public List<BlogDataCategoryViewModel> ArchiveList { get; set; }
    }
}
