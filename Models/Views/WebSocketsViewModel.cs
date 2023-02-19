namespace aspnetmvc_blog.Models.Views
{
    public class WebSocketsViewModel
    {
        public DateTime PayloadDate { get; set; }
        public string PayloadFrom { get; set; } = string.Empty;
        public string PayloadTo { get; set; } = string.Empty;
        public string PayloadData { get; set; } = string.Empty;
    }
}
