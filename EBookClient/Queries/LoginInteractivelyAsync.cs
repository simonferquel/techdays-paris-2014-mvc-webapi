using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace EBookClient.Queries
{
    public class LoginInteractivelyAsync
    {
        public async Task<string> ExecuteAsync(WebView webView, Grid owner)
        {
            var url = string.Format("{0}?response_type=code&client_id={1}&redirect_uri={2}", OAuthSettings.AuthorizeEndpoint,
               Uri.EscapeDataString(OAuthSettings.ClientId),
               Uri.EscapeDataString("https://returnurl"));


            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();


            webView.NavigationStarting += async (s, a) =>
            {
                if (a.Uri.ToString().StartsWith("https://returnurl"))
                {
                    // detect return url
                    owner.Children.Remove(webView);
                    if (a.Uri.Query.StartsWith("?code="))
                    {
                        var code = Uri.UnescapeDataString(a.Uri.Query.Substring(6));
                        using (var client = new HttpClient())
                        {
                            var content = new HttpFormUrlEncodedContent(new Dictionary<string, string>{
                                {"grant_type","authorization_code"},
                                {"code", code},
                                {"redirect_uri", "https://returnurl"},
                                {"client_id", OAuthSettings.ClientId},
                                {"client_secret", OAuthSettings.ClientSecret}
                            });
                            // exchange authorize code for an access token
                            var response = await client.PostAsync(new Uri(OAuthSettings.TokenEndpoint), content);
                            response.EnsureSuccessStatusCode();
                            var contentString = await response.Content.ReadAsStringAsync();
                            var accessTokenInfo = await JsonConvert.DeserializeObjectAsync<OAuthTokenInfo>(contentString);
                            OAuthSettings.AccessToken = accessTokenInfo.AccessToken;
                            OAuthSettings.RefreshToken = accessTokenInfo.RefreshToken;

                            tcs.SetResult(accessTokenInfo.AccessToken);
                        }
                    }
                }
            };
            webView.NavigateWithHttpRequestMessage(new HttpRequestMessage(HttpMethod.Get, new Uri(url)));


            return await tcs.Task;
        }
    }
}
