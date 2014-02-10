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
    [Table("T_EbookParts")]
    public class EbookPart
    {
        [Key]
        [Column(Order = 0)]
        public Guid EbookId { get; set; }

        [ForeignKey("EbookId")]
        public Ebook Ebook { get; set; }

        [Key]
        [Column(Order = 1)]
        public int Position { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public byte[] PartContent { get; set; }
    }
}
