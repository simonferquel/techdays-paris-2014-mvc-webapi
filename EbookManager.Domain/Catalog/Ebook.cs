using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookManager.Domain.Catalog
{
    [Table("T_Ebooks")]
    public class Ebook
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Summary { get; set; }

        public byte[] Thumbnail { get; set; }

        public ICollection<EbookPart> Parts { get; set; }
    }
}
