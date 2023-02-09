using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models.Views
{
    public class ChangePasswordViewModel
    {
        public Guid UserId { get; set; }

        [DisplayName("Username")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Old Password")]
        public string OldPassword { get; set; }

        [NotMapped]
        [RegularExpression("(?=.*?[a-z])(?=.*?[A-Z]).{16,}", ErrorMessage = "Not Valid Password")]
        [Required(ErrorMessage = "Required")]
        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [NotMapped]
        [Required]
        [Compare("NewPassword")]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
