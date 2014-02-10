using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbookManager.Domain.Catalog;

namespace EbookManager.CommonViewModels
{
    public class EbookViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Titre")]
        public string Title { get; set; }

        [Display(Name = "Résumé")]
        public string Summary { get; set; }

        public string Thumbnail { get; set; }

        [Display(Name = "Nombre de partie(s)")]
        public int PartsCount { get; set; }

        public static EbookViewModel FromEbook(Ebook ebook)
        {
            return new EbookViewModel()
            {
                Id = ebook.Id,
                PartsCount = ebook.Parts != null ? ebook.Parts.Count : 0,
                Summary = ebook.Summary,
                Thumbnail = Convert.ToBase64String(ebook.Thumbnail),
                Title = ebook.Title
            };
        }
    }
}
