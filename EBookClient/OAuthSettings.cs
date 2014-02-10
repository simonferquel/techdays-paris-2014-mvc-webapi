using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace EBookClient
{
    class OAuthSettings
    {

        public static readonly string AuthorizeEndpoint = "http://localhost:20394/oauth/authorize";
        public static readonly string TokenEndpoint = "http://localhost:20394/oauth/token";
        public static readonly string ClientId = "win8client";
        public static readonly string ClientSecret = "oauthcadeboite";

        public static string AccessToken { get; set; }

        public static string RefreshToken
        {
            get
            {
                return ApplicationData.Current.LocalSettings.Values["Auth.RefreshToken"] as string;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values["Auth.RefreshToken"] = value;
            }
        }
    }
}
