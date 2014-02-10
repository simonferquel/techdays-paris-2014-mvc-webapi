using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbookManager.Domain.Catalog;

namespace EbookManager.CommonViewModels
{
    public class EbookPartViewModel
    {
        public Guid EbookId { get; set; }
        public int Position { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }

        public static EbookPartViewModel FromEbookPart(EbookPart part)
        {
            return new EbookPartViewModel()
            {
                ContentType = part.ContentType,
                EbookId = part.EbookId,
                Position = part.Position,
                FileName = part.FileName
            };
        }
    }
}
