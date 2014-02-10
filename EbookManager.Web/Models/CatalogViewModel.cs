using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EbookManager.CommonViewModels;

namespace EbookManager.Web.Models
{
    public class CatalogViewModel
    {
        public CatalogViewModel()
        {
            this.Ebooks = new List<EbookViewModel>();
        }

        public List<EbookViewModel> Ebooks { get; set; }
    }
}