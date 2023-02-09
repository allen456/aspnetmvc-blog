using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models
{
    public class UserGrant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserGrantId { get; set; }
        public DateTime UserGrantDate { get; set; }

        [DisplayName("User")]
        public Guid SourceId { get; set; }
        public virtual User? SourceUser { get; set; }

        [DisplayName("Grant")]
        public Guid TargetId { get; set; }
        public virtual User? TargetUser { get; set; }
    }
}
