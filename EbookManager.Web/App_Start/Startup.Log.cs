using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EbookManager.Web.Owin;
using Owin;

namespace EbookManager.Web
{
    public partial class Startup
    {
        public void ConfigureLogMiddleware(IAppBuilder app)
        {
            app.Use(typeof (LogMiddleware));
        }
    }
}