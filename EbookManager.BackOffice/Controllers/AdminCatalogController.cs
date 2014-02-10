using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EbookManager.BackOffice.Models;
using EbookManager.CommonViewModels;
using EbookManager.Domain.Catalog;
using EbookManager.Repositories;

namespace EbookManager.BackOffice.Controllers
{
    [RoutePrefix("catalog")]
    public class AdminCatalogController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Index()
        {
            using (var db = new EbookManagerDbContext())
            {
                var catalogRepository = new CatalogRepository(db);
                var ebooks = await catalogRepository.LoadCatalogAsync();

                var model = new AdminCatalogModel();
                model.Ebooks = ebooks.Select(e => EbookViewModel.FromEbook(e)).ToList();

                return View(model);
            }
        }

        [HttpGet]
        [Route("new-ebook")]
        public ActionResult Add()
        {
            var model = new AddEbookModel();
            return View(model);
        }

        [HttpPost]
        [Route("new-ebook")]
        public async Task<ActionResult> Add(AddEbookModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var db = new EbookManagerDbContext())
            {
                var catalogRepository = new CatalogRepository(db);
                var ebook = new Ebook();
                ebook.Id = Guid.NewGuid();
                ebook.Summary = model.Summary;
                ebook.Title = model.Title;

                ebook.Thumbnail = new byte[model.Thumbnail.ContentLength];
                model.Thumbnail.InputStream.Read(ebook.Thumbnail, 0, ebook.Thumbnail.Length);

                try
                {
                    await catalogRepository.AddEbookAsync(ebook);
                    return RedirectToRoute("editEbook", new { ebookId = ebook.Id });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("InsertError", e);
                    return View(model);
                }
            }
        }

        [HttpGet]
        [Route("edit-ebook/{ebookId}", Name = "editEbook")]
        public async Task<ActionResult> EditEBook(Guid ebookId)
        {
            using (var db = new EbookManagerDbContext())
            {
                var catalogRepository = new CatalogRepository(db);
                var ebook = await catalogRepository.GetEbookAsync(ebookId);

                if (ebook == null)
                    throw new HttpException(404, "not found");

                var model = new EditEbookModel();
                model.Id = ebook.Id;
                model.Summary = ebook.Summary;
                model.Base64Thumbnail = Convert.ToBase64String(ebook.Thumbnail);
                model.Title = ebook.Title;
                model.PartsCount = ebook.Parts.Count;

                foreach (var part in ebook.Parts)
                {
                    model.ExistingParts.Add(EbookPartViewModel.FromEbookPart(part));
                }

                return View(model);
            }
        }

        [HttpPost]
        [Route("edit-ebook")]
        public async Task<ActionResult> EditEBook(EditEbookModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var db = new EbookManagerDbContext())
            {
                var catalogRepository = new CatalogRepository(db);
                var ebook = await catalogRepository.GetEbookAsync(model.Id);

                if (ebook == null)
                    throw new HttpException(404, "not found");

                ebook.Title = model.Title;
                ebook.Summary = model.Summary;

                if (model.Thumbnail != null)
                {
                    ebook.Thumbnail = new byte[model.Thumbnail.ContentLength];
                    model.Thumbnail.InputStream.Read(ebook.Thumbnail, 0, ebook.Thumbnail.Length);
                }

                if (model.Parts != null)
                {
                    foreach (var part in model.Parts)
                    {
                        if (part == null)
                            continue;

                        var ebookPart = new EbookPart();
                        ebookPart.EbookId = ebook.Id;
                        ebookPart.PartContent = new byte[part.ContentLength];
                        part.InputStream.Read(ebookPart.PartContent, 0, ebookPart.PartContent.Length);
                        ebookPart.Position = ebook.Parts.Count;
                        ebookPart.ContentType = part.ContentType;
                        ebookPart.FileName = System.IO.Path.GetFileName(part.FileName);

                        ebook.Parts.Add(ebookPart);
                    }
                }

                await catalogRepository.UpdateBookAsync(ebook);

                TempData["Success"] = "L'eBook a été sauvegardé";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("delete-ebook/{ebookId}", Name = "deleteEbook")]
        public async Task<ActionResult> DeleteBook(Guid ebookId)
        {
            using (var db = new EbookManagerDbContext())
            {
                var catalogRepository = new CatalogRepository(db);
                var ebook = await catalogRepository.GetEbookAsync(ebookId);

                if (ebook == null)
                    throw new HttpException(404, "not found");

                var model = EbookViewModel.FromEbook(ebook);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete-ebook/{ebookId}", Name = "confirmDeleteEbook")]
        public async Task<ActionResult> ConfirmDelete(Guid ebookId)
        {
            using (var db = new EbookManagerDbContext())
            {
                var catalogRepository = new CatalogRepository(db);
                await catalogRepository.DeleteEbookAsync(ebookId);

                TempData["Success"] = "L'eBook a été supprimé";
                return RedirectToAction("Index");
            }
        }
    }
}