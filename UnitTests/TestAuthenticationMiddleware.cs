using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class TestAuthenticationOptions : AuthenticationOptions
    {
        ClaimsIdentity _identity;
        public ClaimsIdentity Identity { get { return _identity; } }

        public TestAuthenticationOptions(ClaimsIdentity identity)
            : base(identity.AuthenticationType)
        {
            _identity = identity;
            base.AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active;
        }

    }

    public static class TestAuthenticationMiddlewareExtensions
    {
        public static IAppBuilder UseTestAuthentication(this IAppBuilder app, ClaimsIdentity identity)
        {
            return app.Use<TestAuthenticationMiddleware>(new TestAuthenticationOptions(identity));
        }
    }
    public class TestAuthenticationMiddleware : AuthenticationMiddleware<TestAuthenticationOptions>
    {
        public TestAuthenticationMiddleware(OwinMiddleware next, TestAuthenticationOptions options)
            : base(next, options)
        {

        }
        protected override AuthenticationHandler<TestAuthenticationOptions> CreateHandler()
        {
            return new TestAuthenticationHandler();
        }
    }

    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            return new AuthenticationTicket(Options.Identity, new AuthenticationProperties());
        }
    }
}
