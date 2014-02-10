using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbookManager.Domain.Users
{
    [Table("T_UserLogins")]
    public class UserLogin
    {
        [Key]
        [Column(Order = 0)]
        [MaxLength(80)]
        public string LoginProvider { get; set; }

        [Key]
        [Column(Order = 1)]
        [MaxLength(120)]
        public string ProviderKey { get; set; }

        [Key]
        [Column(Order = 2)]
        [MaxLength(120)]
        public string UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
