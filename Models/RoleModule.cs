using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models
{
    public class RoleModule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid RoleModuleId { get; set; }
         
        [DisplayName("Order")]
        public int RoleModuleOrder { get; set; }

        [DisplayName("Access")]
        public bool RoleModuleAccess { get; set; }

        [DisplayName("Role")]
        public Guid RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }

        [DisplayName("Module")]
        public Guid ModuleId { get; set; }
        [ForeignKey("ModuleId")]
        public virtual Module? Module { get; set; }
    }
}
