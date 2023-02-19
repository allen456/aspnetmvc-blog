using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ModuleId { get; set; }
        public DateTime ModuleCreateDate { get; set; }

        [DisplayName("Order")]
        public int ModuleOrder { get; set; }

        [Required]
        [DisplayName("Module Name")]
        public string ModuleName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Controller")]
        public string ModuleController { get; set; } = string.Empty;

        [Required]
        [DisplayName("Action")]
        public string ModuleAction { get; set; } = string.Empty;

        [Required]
        [DisplayName("Menu")]
        public bool ModuleVisibility { get; set; }

        [Required]
        [DisplayName("Icon")]
        public string ModuleIconClass { get; set; } = string.Empty;

        [Required]
        [DisplayName("Tooltip Text")]
        public string ModuleTooltip { get; set; } = string.Empty;

        public ICollection<RoleModule> RoleModule { get; set; } = new List<RoleModule>();
        public ICollection<UserModule> UserModule { get; set; } = new List<UserModule>();
    }
}
