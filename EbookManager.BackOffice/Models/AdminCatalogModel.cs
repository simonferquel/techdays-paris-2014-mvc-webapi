using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EbookManager.CommonViewModels;

namespace EbookManager.BackOffice.Models
{
    public class AdminCatalogModel
    {
        public List<EbookViewModel> Ebooks { get; set; }
    }
}