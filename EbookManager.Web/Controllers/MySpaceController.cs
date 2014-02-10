using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EbookManager.CommonViewModels;
using EbookManager.Repositories;
using EbookManager.Web.Models;

namespace EbookManager.Web.Controllers
{
    [RoutePrefix("my")]
    public class MySpaceController : Controller
    {
        [Route("")]
        public async Task<ActionResult> Index()
        {
            using (var db = new EbookManagerDbContext())
            {
                var userName = User.Identity.Name;

                var catalogRepository = new CatalogRepository(db);
                var ebooks = await catalogRepository.LoadUserCatalog(userName);

                var model = new CatalogViewModel();
                model.Ebooks.AddRange(ebooks.Select(e => EbookViewModel.FromEbook(e)));

                return View(model);
            }
        }
	}
}