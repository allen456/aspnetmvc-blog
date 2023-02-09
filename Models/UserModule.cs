using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models
{
    public class UserModule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid UserModuleId { get; set; }
         
        [DisplayName("Order")]
        public int UserModuleOrder { get; set; }

        [DisplayName("Access")]
        public bool UserModuleAccess { get; set; } 

        [DisplayName("User")]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [DisplayName("Module")]
        public Guid ModuleId { get; set; }
        [ForeignKey("ModuleId")]
        public virtual Module? Module { get; set; }
    }
}
