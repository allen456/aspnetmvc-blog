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
        public string AppLogUser { get; set; } = string.Empty;
        public string AppLogController { get; set; } = string.Empty;
        public string AppLogAction { get; set; } = string.Empty;
        public string AppLogQuery { get; set; } = string.Empty;
        public string AppLogAddress { get; set; } = string.Empty;
    }
}
 