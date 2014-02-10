using System;
using EbookManager.Web.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Owin;
using System.Security.Claims;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Dispatcher;
using System.Collections.Generic;
using System.Reflection;

namespace UnitTests
{
    [TestClass]
    public class WebApiTests
    {
        static ClaimsIdentity _testIdentity;
        class OwinTestConf
        {
            public void Configuration(IAppBuilder app)
            {
                app.Use(typeof (LogMiddleware));
                app.UseTestAuthentication(_testIdentity);
                HttpConfiguration config = new HttpConfiguration();
                config.Services.Replace(typeof(IAssembliesResolver), new TestWebApiResolver());
                config.MapHttpAttributeRoutes();
                app.UseWebApi(config);
            }
        }

       

        class WhoAmiResult
        {
            public string UserName { get; set; }
            public string NameIdentifier { get; set; }
        }

        [TestMethod]
        public async Task Test_WhoAmi()
        {
            _testIdentity = new ClaimsIdentity("testidentity");
            _testIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "test name identifier"));
            _testIdentity.AddClaim(new Claim(ClaimTypes.Name, "test user"));
            using (var server = TestServer.Create<OwinTestConf>())
            {
                using (var client = new HttpClient(server.Handler))
                {
                    var response = await client.GetAsync("http://testserver/api/user/whoami");
                    var result = await response.Content.ReadAsAsync<WhoAmiResult>();
                    Assert.AreEqual("test user", result.UserName);
                    Assert.AreEqual("test name identifier", result.NameIdentifier);
                }
            }
        }
    }
}
