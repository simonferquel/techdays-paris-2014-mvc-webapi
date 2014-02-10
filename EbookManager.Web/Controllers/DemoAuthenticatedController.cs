using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbookManager.Web.Controllers
{
    [Authorize]
    [RoutePrefix("demowebviewauth")]
    public class DemoAuthenticatedController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
	}
}