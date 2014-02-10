using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EbookManager.Web.Startup))]
namespace EbookManager.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Configuration du middleware de logs OWIN
            ConfigureLogMiddleware(app);

            //Configuration de l'authentification Web via OWIN
            ConfigureWebAuth(app);

            //Configuration de l'authentification OAuth2 via OWIN
            ConfigureOAuth2(app);
        }
    }
}
