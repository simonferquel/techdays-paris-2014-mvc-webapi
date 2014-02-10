using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbookManager.Domain.Users;

namespace EbookManager.Domain.Catalog
{
    [Table("T_UserEbooks")]
    public class UserEbook
    {
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }

        [ForeignKey("EbookId")]
        public Ebook Ebook { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid EbookId { get; set; }
    }
}
