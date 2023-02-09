using aspnetmvc_blog.Models;
using aspnetmvc_blog.Models.Views;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace aspnetmvc_blog.Data
{
    public class LibraryBlog
    {
        public LibraryBlog(IConfiguration config) { _config = config; }
        private readonly IConfiguration _config;

        public async Task<List<BlogDataViewModel>> getBlogList()
        {
            List<BlogDataViewModel> blogList = new();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(_config.GetValue<string>("BlogApi"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                blogList = JsonConvert.DeserializeObject<List<BlogDataViewModel>>(apiResponse);
            }
            return blogList;
        }
    }
}
