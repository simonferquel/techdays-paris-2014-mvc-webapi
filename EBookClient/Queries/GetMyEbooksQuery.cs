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
    public class EbookResume
        {
            public string Id { get; set; }
            public string Summary { get; set; }
            public string Title { get; set; }
            public byte[] Thumbnail { get; set; }
            public int PartCount { get; set; }
        }
    public class GetMyEbooksQuery
    {
        public async Task<List<EbookResume>> ExecuteAsync(string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Bearer", accessToken);
                var responseString = await client.GetStringAsync(new Uri("http://localhost:20394/api/ebooks/my"));
                return  await JsonConvert.DeserializeObjectAsync<List<EbookResume>>(responseString);
                
            }
        }
    }
}
