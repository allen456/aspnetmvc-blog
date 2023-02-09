namespace aspnetmvc_blog.Models.Views
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ErrorType { get; set; }
        public string ReturnUrl { get; set; }
    }
}