using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EbookManager.Repositories;

namespace EbookManager.BackOffice
{
    /// <summary>
    /// Summary description for EbookPart
    /// </summary>
    public class EbookPartFileHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Guid ebookId;
            int position;
            if (context.Request.QueryString.AllKeys.Contains("ebook") 
                && Guid.TryParse(context.Request.QueryString["ebook"], out ebookId)
                && context.Request.QueryString.AllKeys.Contains("position")
                && int.TryParse(context.Request.QueryString["position"], out position))
            {
                using (var db = new EbookManagerDbContext())
                {
                    var catalogRepository = new CatalogRepository(db);
                    var ebookPart = catalogRepository.GetEbookPart(ebookId, position);
                    if (ebookPart != null)
                    {
                        context.Response.Clear();
                        context.Response.ContentType = ebookPart.ContentType;
                        context.Response.AddHeader("Content-Disposition", "attachment; filename=" + ebookPart.FileName);
                        context.Response.BinaryWrite(ebookPart.PartContent);
                        context.Response.End();
                        return;
                    }
                }
            }

            context.Response.StatusCode = 404;
            context.Response.StatusDescription = "Not Found";
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}