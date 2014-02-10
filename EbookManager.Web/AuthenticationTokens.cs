using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbookManager.Web
{
    public class AuthorizationCodeProvider : AuthenticationTokenProvider
    {
        public override async System.Threading.Tasks.Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            context.SetToken(context.SerializeTicket());
        }

        public override async System.Threading.Tasks.Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }

    public class AccessTokenProvider : AuthenticationTokenProvider
    {
        public override async System.Threading.Tasks.Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            context.SetToken(context.SerializeTicket());
        }

        public override async System.Threading.Tasks.Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        public override async System.Threading.Tasks.Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            // refresh tokens don't expire
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddYears(100);
            context.SetToken(context.SerializeTicket());
        }

        public override async System.Threading.Tasks.Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}