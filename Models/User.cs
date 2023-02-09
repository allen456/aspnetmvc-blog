using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnetmvc_blog.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }
        public bool UserStatus { get; set; }
        public DateTime UserCreateDate { get; set; }

        public bool PasswordChange { get; set; }
        public DateTime PasswordChangeDate { get; set; }

        [Required]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
        public ICollection<UserGroup> UserGroup { get; set; } = new List<UserGroup>();
        public ICollection<UserModule> UserModule { get; set; } = new List<UserModule>();
        public virtual ICollection<UserGrant> SourceGrant { get; set; } = new List<UserGrant>();
        public virtual ICollection<UserGrant> TargetGrant { get; set; } = new List<UserGrant>();
    }
}
