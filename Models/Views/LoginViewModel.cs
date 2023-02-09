using System.ComponentModel.DataAnnotations;

namespace aspnetmvc_blog.Models.Views
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
