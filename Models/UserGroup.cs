using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models
{
    public class UserGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid UserGroupId { get; set; }
        
        [DisplayName("Order")]
        public int UserGroupOrder { get; set; }

        [DisplayName("User")]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [DisplayName("Group")]
        public Guid GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group? Group { get; set; }
    }
}
