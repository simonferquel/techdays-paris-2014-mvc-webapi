using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EbookManager.CommonViewModels;

namespace EbookManager.BackOffice.Models
{
    public class EditEbookModel : AddEbookModel
    {
        public EditEbookModel()
        {
            ExistingParts = new List<EbookPartViewModel>();
        }

        public string Base64Thumbnail { get; set; }

        [Display(Name = "Partie(s)")]
        public List<HttpPostedFileBase> Parts { get; set; }

        public List<EbookPartViewModel> ExistingParts { get; set; }

        public int PartsCount { get; set; }
    }
}