using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models.Views;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace aspnetmvc_blog.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(LibraryBlog bloglib)
        {
            _bloglib = bloglib;
        }
        private readonly LibraryBlog _bloglib;
        public readonly int _featurecount = 2;
        public readonly int _homecount = 3;

        public async Task<IActionResult> IndexAsync()
        {
            List<BlogDataViewModel> blogList = await _bloglib.getBlogList();
            BlogHomeViewModel blogDisplay = new();
            var topFeatured = blogList.Where(w => w.Feature).OrderByDescending(q => q.BlogDate).First();
            var featured = blogList.Where(w => w.Feature && w._id != topFeatured._id).OrderByDescending(q => q.BlogDate).Take(_featurecount).ToList();
            blogDisplay.TopBlog = topFeatured;
            blogDisplay.FeaturedBlog = featured;
            blogDisplay.NewestBlog = blogList.OrderByDescending(q => q.BlogDate).Take(_homecount).ToList();
            blogDisplay.ArchiveList = blogList.GroupBy(d => new { MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.BlogDate.Month), d.BlogDate.Year, d.BlogDate.Month }).Select(g => new BlogDataCategoryViewModel { name = string.Format("{1} {0}", g.Key.MonthName, g.Key.Year), month = g.Key.Month, year = g.Key.Year }).ToList();
            return View(blogDisplay);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string type, string url)
        {
            return View(new ErrorViewModel { ErrorType = type, ReturnUrl = url });
        }

    }
}