using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoleId { get; set; }
        public bool RoleStatus { get; set; }
        public DateTime RoleCreateDate { get; set; }

        [Required]
        [DisplayName("Name")]
        public string RoleName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Administrator")]
        public bool AdministratorFlag { get; set; }

        [Required]
        [DisplayName("Register")] 
        public bool RegisterFlag { get; set; }

        [Required]
        [DisplayName("Default action")]
        public string DefaultAction { get; set; } = string.Empty;

        [Required]
        [DisplayName("Default controller")]
        public string DefaultController { get; set; } = string.Empty;

        public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
        public ICollection<RoleModule> RoleModule { get; set; } = new List<RoleModule>();
    }
}
