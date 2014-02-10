using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbookManager.Domain.Users
{
    [Table("T_Roles")]
    public class Role
    {
        [Key]
        [MaxLength(80)]
        public string Id { get; set; }
        
        [Required]
        [MaxLength(120)]
        public string Name { get; set; }
    }
}
