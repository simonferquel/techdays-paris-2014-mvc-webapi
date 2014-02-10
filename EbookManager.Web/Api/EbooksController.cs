using EbookManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EbookManager.Web.Api
{
    [Authorize]
    [RoutePrefix("api/ebooks")]
    public class EbooksController : ApiController
    {
        [HttpGet]
        [Route("my")]
        public async Task<HttpResponseMessage> GetMyEbooks()
        {
            using (var db = new EbookManagerDbContext())
            {
                var userName = User.Identity.Name;

                var catalogRepository = new CatalogRepository(db);
                var ebooks = await catalogRepository.LoadUserCatalogWithPartCount(userName);
                return Request.CreateResponse(ebooks.Select(e => new { Id = e.Item1.Id, Summary = e.Item1.Summary, Title = e.Item1.Title, Thumbnail = e.Item1.Thumbnail, PartCount = e.Item2 }));
            }
        }

        [HttpGet]
        [Route("ebook/{ebookId}/part/{index}")]
        public async Task<HttpResponseMessage> GetEbookPart(Guid ebookId, int index)
        {
            using (var db = new EbookManagerDbContext())
            {
                var catalogRepository = new CatalogRepository(db);
                var part = catalogRepository.GetEbookPart(ebookId, index);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(part.PartContent);
                response.Content.Headers.ContentLength = part.PartContent.Length;
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(part.ContentType);
                return response;
            }
        }

        [HttpGet]
        [Route("")]
        [Queryable]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetEbooks()
        {
            using (var db = new EbookManagerDbContext())
            {
                var userName = "julien";
                var catalogRepository = new CatalogRepository(db);
                var userEbooks = await catalogRepository.LoadUserCatalog(userName);

                return Request.CreateResponse(HttpStatusCode.OK, userEbooks);
            }
        }
    }
}
