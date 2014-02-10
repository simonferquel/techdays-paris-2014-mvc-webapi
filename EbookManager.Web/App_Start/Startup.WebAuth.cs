using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace EbookManager.Web
{
    public partial class Startup
    {
        public void ConfigureWebAuth(IAppBuilder app)
        {
            //Utilisation de cookie pour stocker les informations sur les utilisateurs authentifiés
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Account/Login"),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            // Utilisation d'un cookie pour le stockage des info temporaires des utilisateurs authentifiés
            // via un fournisseur externe
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Activation du fournisseur d'authentification Microsoft
            app.UseMicrosoftAccountAuthentication(
                clientId: "00000000481106FF",
                clientSecret: "MqwDmwpeGTezV6o4sJdgVKIfZm76TZt6");

            // Activation du fournisseur d'authentification Facebook
            app.UseFacebookAuthentication(
               appId: "636293909765104",
               appSecret: "807a0a17eeaa20dabc1bd72f1be33775");

            // Activation du fournisseur d'authentification Google
            app.UseGoogleAuthentication();
        }
    }
}