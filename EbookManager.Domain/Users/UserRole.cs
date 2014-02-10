using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbookManager.Domain.Users
{
    [Table("T_UserRoles")]
    public class UserRole
    {
        [Key]
        [Column(Order = 0)]
        [MaxLength(120)]
        public string UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Key]
        [Column(Order = 1)]
        [MaxLength(80)]
        public string RoleId { get; set; }
        
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
