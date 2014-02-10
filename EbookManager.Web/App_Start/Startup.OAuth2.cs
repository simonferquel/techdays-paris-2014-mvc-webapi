using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace EbookManager.Web
{
    public partial class Startup
	{
        public static OAuthAuthorizationServerOptions OAuthServerOptions { get; set; }

        static Startup()
        {
            OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                Provider = new OAuthServerProvider(),
                AuthorizeEndpointPath = new PathString("/oauth/authorize"),
                TokenEndpointPath = new PathString("/oauth/token"),
                AuthorizationCodeProvider = new AuthorizationCodeProvider(),
                AccessTokenProvider = new AccessTokenProvider(),
                RefreshTokenProvider = new RefreshTokenProvider()
            };
        }

        public void ConfigureOAuth2(IAppBuilder app)
        {
            // Activation de l'authentification via OAuth2
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = OAuthServerOptions.AccessTokenFormat,
                AccessTokenProvider = OAuthServerOptions.AccessTokenProvider,
            }); 
        }
	}
}