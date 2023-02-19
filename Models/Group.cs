using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GroupId { get; set; }
        public bool GroupStatus { get; set; }
        public DateTime GroupCreateDate { get; set; }

        [Required]
        [DisplayName("Code")]
        public string GroupCode { get; set; } = string.Empty;
         
        [Required]
        [DisplayName("Description")]
        public string GroupDescription { get; set; } = string.Empty;

        public ICollection<UserGroup> UserGroup { get; set; } = new List<UserGroup>();
    }
}
