using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models
{
    public class AppLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AppLogId { get; set; }
        public DateTime AppLogDate { get; set; }
        public string AppLogUser { get; set; }
        public string AppLogController { get; set; }
        public string AppLogAction { get; set; }
        public string AppLogQuery { get; set; }
        public string AppLogAddress { get; set; } 
    }
}
 