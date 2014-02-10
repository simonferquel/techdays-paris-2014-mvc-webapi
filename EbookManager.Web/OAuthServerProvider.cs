using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EbookManager.Web
{
    public class OAuthServerProvider : OAuthAuthorizationServerProvider
    {
        // /oauth/authorize
        public override async Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        {
            if (context.Request.User != null && context.Request.User.Identity.IsAuthenticated)
            {
                // si l'utilisateur est loggé,
                // on crée un ticket d'authent, on crée un authorization code et on fait le redirect
                var redirectUri = context.Request.Query["redirect_uri"];
                var clientId = context.Request.Query["client_id"];
                var authorizeCodeContext = new Microsoft.Owin.Security.Infrastructure.AuthenticationTokenCreateContext(context.OwinContext, context.Options.AuthorizationCodeFormat,
                    new Microsoft.Owin.Security.AuthenticationTicket((ClaimsIdentity)context.Request.User.Identity, new Microsoft.Owin.Security.AuthenticationProperties(new Dictionary<string, string>
                    {
                        {"client_id", clientId},
                        {"redirect_uri", redirectUri}
                    })
                    {
                        IssuedUtc = DateTimeOffset.UtcNow,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(context.Options.AuthorizationCodeExpireTimeSpan)
                    }));
                await context.Options.AuthorizationCodeProvider.CreateAsync(authorizeCodeContext);
                
                // clear cookies
                var cookies = context.Request.Cookies.ToList();
                foreach (var c in cookies)
                {
                    context.Response.Cookies.Delete(c.Key, new Microsoft.Owin.CookieOptions());
                }
                context.Response.Redirect(redirectUri + "?code=" + Uri.EscapeDataString(authorizeCodeContext.Token));
            }
            else
            {
                // si on n'est pas loggé, on redirige vers la page de login
                context.Response.Redirect("/account/login?returnUrl=" + Uri.EscapeDataString(context.Request.Uri.ToString()));
            }
            context.RequestCompleted();
        }
        public override async Task ValidateAuthorizeRequest(OAuthValidateAuthorizeRequestContext context)
        {
            // validation d'une authorize request
            if (context.AuthorizeRequest.ClientId == "win8client" && context.AuthorizeRequest.IsAuthorizationCodeGrantType)
            {
                context.Validated();
            }
            else
            {
                context.Rejected();
            }
        }

        public override async Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            // appelé pour valider la redirect_uri. 
            // dans la vraie vie, on valide vraiment :)
            context.Validated(context.RedirectUri);
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // appelé pour valider que le client id et client secret sont valides
            string clientId;
            string clientSecret;
            if (context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                if (clientId == "win8client" && clientSecret == "oauthcadeboite")
                {
                    context.Validated(clientId);
                    return;
                }
            }

            context.Rejected();

        }
        public override async Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        {
            // valide la requète de token
            // dans note cas on accepte les requètes de type "authorize code" et "refresh_token"
            if (context.TokenRequest.IsAuthorizationCodeGrantType || context.TokenRequest.IsRefreshTokenGrantType)
            {
                context.Validated();
            }
            else
            {
                context.Rejected();
            }
        }
    }
}