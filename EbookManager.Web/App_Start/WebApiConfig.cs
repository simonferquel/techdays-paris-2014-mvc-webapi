using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Batch;

namespace EbookManager.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            #region Web API Batching
            // Web API configuration and services
            var batchHandler = new DefaultHttpBatchHandler(GlobalConfiguration.DefaultServer)
            {
                ExecutionOrder = BatchExecutionOrder.NonSequential
            };
            config.Routes.MapHttpBatchRoute(
                routeName: "WebApiBatch",
                routeTemplate: "api/batch",
                batchHandler: batchHandler); 
            #endregion

            #region Authentification

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(Startup.OAuthServerOptions.AuthenticationType));

            #endregion

            #region Attribute Routing
            // Web API routes
            config.MapHttpAttributeRoutes(); 
            #endregion
        }
    }
}
