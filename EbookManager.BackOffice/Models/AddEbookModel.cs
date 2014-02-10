using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbookManager.BackOffice.Models
{
    public class AddEbookModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [Display(Name = "Titre")]
        [Required(ErrorMessage = "*")]
        [StringLength(120, ErrorMessage = "Maximum 120 caractères")]
        public string Title { get; set; }

        [Display(Name = "Résumé")]
        [Required(ErrorMessage = "*")]
        [StringLength(500, ErrorMessage = "Maximum 500 caractères")]
        public string Summary { get; set; }

        [Display(Name = "Vignette")]
        public HttpPostedFileBase Thumbnail { get; set; }
    }
}