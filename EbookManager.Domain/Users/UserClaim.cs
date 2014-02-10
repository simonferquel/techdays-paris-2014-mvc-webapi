using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbookManager.Domain.Users
{
    [Table("T_UserClaims")]
    public class UserClaim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string ClaimType { get; set; }


        [Required]
        [MaxLength(120)]
        public string ClaimValue { get; set; }


        [Required]
        [MaxLength(120)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
