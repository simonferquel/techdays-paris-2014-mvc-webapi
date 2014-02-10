using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EbookManager.Domain.Catalog;
using Microsoft.AspNet.Identity;

namespace EbookManager.Domain.Users
{
    [Table("T_Users")]
    public class User : IUser
    {
        public User() { }

        public User(string userName)
        {
            this.UserName = userName;
        }

        [Key]
        [MaxLength(120)]
        public string Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string UserName { get; set; }

        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(255)]
        public string SecurityStamp { get; set; }
        
        public ICollection<UserClaim> Claims { get; set; }
        public ICollection<UserLogin> Logins { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public ICollection<UserEbook> Ebooks { get; set; }
    }
}
