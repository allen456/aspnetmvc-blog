using aspnetmvc_blog.Models;
using aspnetmvc_blog.Models.Views;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

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

        public async Task<BlogDataViewModel> getBlogData(string id)
        {
            BlogDataViewModel blogData = new();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(_config.GetValue<string>("BlogApi") + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                blogData = JsonConvert.DeserializeObject<BlogDataViewModel>(apiResponse);
            }
            return blogData;
        }

        public async Task<BlogDataViewModel> sendBlogData(BlogDataPostViewModel newBlogData)
        {
            BlogDataViewModel blogData = new();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new(JsonConvert.SerializeObject(newBlogData), Encoding.UTF8, "application/json");
                using var response = await httpClient.PostAsync(_config.GetValue<string>("BlogApi"), content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                blogData = JsonConvert.DeserializeObject<BlogDataViewModel>(apiResponse);
            }
            return blogData;
        }

        public async Task<BlogDataViewModel> updateBlogData(string id, BlogDataPostViewModel newBlogData)
        {
            BlogDataViewModel blogData = new();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new(JsonConvert.SerializeObject(newBlogData), Encoding.UTF8, "application/json");
                using var response = await httpClient.PatchAsync(_config.GetValue<string>("BlogApi") + "/" + id, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                blogData = JsonConvert.DeserializeObject<BlogDataViewModel>(apiResponse);
            }
            return blogData;
        }
    }
}
