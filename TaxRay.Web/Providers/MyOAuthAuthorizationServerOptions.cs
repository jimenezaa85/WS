using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace TaxRay.Web.Providers
{
    public class MyOAuthAuthorizationServerOptions : IOAuthAuthorizationServerOptions
    {
        private readonly IOAuthAuthorizationServerProvider _provider;
        private readonly IAuthenticationTokenProvider _tokenProvider;

        public MyOAuthAuthorizationServerOptions(IAuthenticationTokenProvider tProvider,
            IOAuthAuthorizationServerProvider provider)
        {
            _provider = provider;
            _tokenProvider = tProvider;
        }
        public OAuthAuthorizationServerOptions GetOptions()
        {
            return new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true, //TODO: HTTPS
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = _provider,
                RefreshTokenProvider = _tokenProvider
            };
        }
    }
}