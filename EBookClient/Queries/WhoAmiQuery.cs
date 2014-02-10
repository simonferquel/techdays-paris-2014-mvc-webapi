using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace EBookClient.Queries
{
    public class WhoAmiQuery
    {
        public async Task<UserInfo> ExecuteAsync(string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Bearer", accessToken);
                var responseString = await client.GetStringAsync(new Uri("http://localhost:20394/api/user/whoami"));
                return await JsonConvert.DeserializeObjectAsync<UserInfo>(responseString);
            }
        }
    }
}
