using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EBookClient.Queries
{
    public class GetAllEbookPartsQuery
    {
        public async Task ExecuteAsync(string accessToken, string ebookId, int partCount, Func<Stream, Task> streamHandler)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, new Uri("http://localhost:20394/api/batch"));
            var content = new MultipartContent("mixed");
            message.Content = content;
            for (int i = 0; i < partCount; ++i)
            {
                content.Add(new HttpMessageContent(
                    new HttpRequestMessage(HttpMethod.Get, new Uri(string.Format("http://localhost:20394/api/ebooks/ebook/{0}/part/{1}", ebookId, i)))));
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.SendAsync(message);

                var streamProvider = await response.Content.ReadAsMultipartAsync();
                foreach (var partialContent in streamProvider.Contents)
                {
                    var part = await partialContent.ReadAsHttpResponseMessageAsync();
                    var partStream = await part.Content.ReadAsStreamAsync();
                    await streamHandler(partStream);
                }
            }
        }
    }
}
