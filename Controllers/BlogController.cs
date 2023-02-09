using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models.Views;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace aspnetmvc_blog.Controllers
{
    public class BlogController : Controller
    {
        public BlogController(LibraryBlog bloglib)
        {
            _bloglib = bloglib;
        }
        private readonly LibraryBlog _bloglib;

        public async Task<IActionResult> Archive(int year, int month)
        {
            List<BlogDataViewModel> blogList = await _bloglib.getBlogList();
            BlogArchiveViewModel blogDisplay = new()
            {
                ArchiveName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + " " + year,
                NewestBlog = blogList.Where(w => w.BlogDate.Month == month && w.BlogDate.Year == year).OrderByDescending(q => q.BlogDate).ToList(),
                ArchiveList = blogList.GroupBy(d => new { MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.BlogDate.Month), d.BlogDate.Year, d.BlogDate.Month }).Select(g => new BlogDataCategoryViewModel { name = string.Format("{1} {0}", g.Key.MonthName, g.Key.Year), month = g.Key.Month, year = g.Key.Year }).ToList()
            };
            return View(blogDisplay);
        }

        public async Task<IActionResult> Category(string id)
        {
            List<BlogDataViewModel> blogList = await _bloglib.getBlogList();
            BlogCategoryViewModel blogDisplay = new()
            {
                CategoryName = id,
                NewestBlog = blogList.Where(w => w.Category == id).OrderByDescending(q => q.BlogDate).ToList(),
                ArchiveList = blogList.GroupBy(d => new { MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.BlogDate.Month), d.BlogDate.Year, d.BlogDate.Month }).Select(g => new BlogDataCategoryViewModel { name = string.Format("{1} {0}", g.Key.MonthName, g.Key.Year), month = g.Key.Month, year = g.Key.Year }).ToList()
            };
            return View(blogDisplay);
        }

        public async Task<IActionResult> Index(string id)
        {
            List<BlogDataViewModel> blogList = await _bloglib.getBlogList();
            BlogDetailedViewModel blogDisplay = new()
            {
                BlogDisplay = blogList.First(w => w._id == id),
                ArchiveList = blogList.GroupBy(d => new { MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.BlogDate.Month), d.BlogDate.Year, d.BlogDate.Month }).Select(g => new BlogDataCategoryViewModel { name = string.Format("{1} {0}", g.Key.MonthName, g.Key.Year), month = g.Key.Month, year = g.Key.Year }).ToList()
            };
            return View(blogDisplay);
        }

        public async Task<IActionResult> List()
        {
            List<BlogDataViewModel> blogList = await _bloglib.getBlogList();
            BlogListViewModel blogDisplay = new()
            {
                NewestBlog = blogList.OrderByDescending(q => q.BlogDate).ToList(),
                ArchiveList = blogList.GroupBy(d => new { MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.BlogDate.Month), d.BlogDate.Year, d.BlogDate.Month }).Select(g => new BlogDataCategoryViewModel { name = string.Format("{1} {0}", g.Key.MonthName, g.Key.Year), month = g.Key.Month, year = g.Key.Year }).ToList()
            };
            return View(blogDisplay);
        }

        [ServiceFilter(typeof(AuthenticationFilter))]
        [ServiceFilter(typeof(LoggingFilter))]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [ServiceFilter(typeof(LoggingFilter))]
        public async Task<IActionResult> Create([Bind("Category,Feature,Image,Title,Subtitle,Content")] BlogDataViewModel data)
        {
            List<BlogDataViewModel> blogList = await _bloglib.getBlogList();
            return View(data);
        }

        [ServiceFilter(typeof(AuthenticationFilter))]
        [ServiceFilter(typeof(LoggingFilter))]
        public IActionResult Edit(string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [ServiceFilter(typeof(LoggingFilter))]
        public async Task<IActionResult> Edit(string id, [Bind("Category,Feature,Image,Title,Subtitle,Content")] BlogDataViewModel data)
        {
            List<BlogDataViewModel> blogList = await _bloglib.getBlogList();
            return View(data);
        }
    }
}
