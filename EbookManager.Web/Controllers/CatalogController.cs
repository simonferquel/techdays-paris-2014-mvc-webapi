using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EbookManager.CommonViewModels;
using EbookManager.Repositories;
using EbookManager.Web.Models;

namespace EbookManager.Web.Controllers
{
    [RoutePrefix("catalog")]
    public class CatalogController : Controller
    {
        [Route("")]
        public async Task<ActionResult> Index()
        {
            using (var db = new EbookManagerDbContext())
            {
                var catalogRepository = new CatalogRepository(db);
                var ebooks = await catalogRepository.LoadCatalogWithoutPartsAsync();

                var model = new CatalogViewModel();
                model.Ebooks.AddRange(ebooks.Select(e => EbookViewModel.FromEbook(e)));

                return View(model);
            }
        }

        [Route("ebook/{ebookId}", Name = "ebookDetails")]
        public async Task<ActionResult> Details(Guid ebookId)
        {
            using (var db = new EbookManagerDbContext())
            {
                var catalogRepository = new CatalogRepository(db);
                var ebook = await catalogRepository.GetEbookAsync(ebookId);

                var model = EbookViewModel.FromEbook(ebook);

                var userId = ClaimsPrincipal.Current.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                ViewBag.UserOwnsBook = await catalogRepository.UserOwnsBookAsync(userId, ebookId);

                return View(model);
            }
        }

        [HttpPost]
        [Route("buy/{ebookId}")]
        public async Task<ActionResult> Buy(Guid ebookId)
        {
            using (var db = new EbookManagerDbContext())
            {
                var catalogRepository = new CatalogRepository(db);

                var userId = ClaimsPrincipal.Current.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                await catalogRepository.BuyEbookAsync(userId, ebookId);

                TempData["Success"] = "Votre achat a été pris en compte !";

                return RedirectToAction("Index", "MySpace");
            }
        }
	}
}