using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models
{ 
    public class AppSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AppSettingId { get; set; }
        public string AppSettingCode { get; set; }
        public string AppSettingValue { get; set; } 
    }
}
 