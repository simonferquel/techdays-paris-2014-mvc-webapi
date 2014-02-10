using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Owin;

namespace EbookManager.Web.Api
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("whoami")]
        public HttpResponseMessage WhoAmi()
        {
            return Request.CreateResponse(new { UserName = User.Identity.Name, NameIdentifier = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value });
        }

       

    }
}
