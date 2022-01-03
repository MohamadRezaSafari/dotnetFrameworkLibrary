using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Health.Providers
{

    /**
     * app_start/startup.auth


	private void CreateRefreshToken(AuthenticationTokenCreateContext context)
	{
		context.SetToken(context.SerializeTicket());
	}

	private void ReceiveRefreshToken(AuthenticationTokenReceiveContext context)
	{
		context.DeserializeTicket(context.Token);
	}

	OAuthOptions = new OAuthAuthorizationServerOptions
	{
		TokenEndpointPath = new PathString("/Token"),
		Provider = new ApplicationOAuthProvider(PublicClientId),
		AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
		AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
		// In production mode set AllowInsecureHttp = false
		AllowInsecureHttp = true,

		RefreshTokenProvider = new AuthenticationTokenProvider
		{
			OnCreate = CreateRefreshToken,
			OnReceive = ReceiveRefreshToken,
		}

	};


web.config

	<machineKey
      validationKey="AutoGenerate,IsolateApps"
      decryptionKey="AutoGenerate,IsolateApps"
      validation="HMACSHA512"
      decryption="AES"
     />
        **/

    /*
     * Startup.Auth.cs
     * --------
         OAuthOptions = new OAuthAuthorizationServerOptions
        {
            TokenEndpointPath = new PathString("/Token"),
            Provider = new ApplicationOAuthProvider(PublicClientId),
            AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
            AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
            // In production mode set AllowInsecureHttp = false
            AllowInsecureHttp = true,
            RefreshTokenProvider = new RefreshTokenProvider()
        };
    */

    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens =
            new ConcurrentDictionary<string, AuthenticationTicket>();

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            await Task.Run(() =>
            {
                var guid = Guid.NewGuid().ToString();

                var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
                {
                    IssuedUtc = context.Ticket.Properties.IssuedUtc,
                    ExpiresUtc = DateTime.UtcNow.AddMonths(3)
                };

                var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

                _refreshTokens.TryAdd(guid, refreshTokenTicket);
                context.SetToken(guid);
            });
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            await Task.Run(() =>
            {
                AuthenticationTicket ticket;

                if (_refreshTokens.TryRemove(context.Token, out ticket))
                {
                    context.SetTicket(ticket);
                }
            });
        }
    }
}