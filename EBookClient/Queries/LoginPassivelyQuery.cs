using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace EBookClient.Queries
{
    public class LoginPassivelyQuery
    {
        public async Task<string> ExecuteAsync()
        {
            if (OAuthSettings.AccessToken != null)
            {
                return OAuthSettings.AccessToken;
            }
            else if (OAuthSettings.RefreshToken == null)
            {
                return null;
            }
            using (var client = new HttpClient())
            {
                var content = new HttpFormUrlEncodedContent(new Dictionary<string, string>{
                                {"grant_type","refresh_token"},
                                {"refresh_token", OAuthSettings.RefreshToken},
                                {"client_id", OAuthSettings.ClientId},
                                {"client_secret", OAuthSettings.ClientSecret}
                            });
                var response = await client.PostAsync(new Uri(OAuthSettings.TokenEndpoint), content);
                response.EnsureSuccessStatusCode();
                var contentString = await response.Content.ReadAsStringAsync();
                var accessTokenInfo = await JsonConvert.DeserializeObjectAsync<OAuthTokenInfo>(contentString);
                OAuthSettings.AccessToken = accessTokenInfo.AccessToken;
                OAuthSettings.RefreshToken = accessTokenInfo.RefreshToken;
                return OAuthSettings.AccessToken;
            }
        }
    }
}
