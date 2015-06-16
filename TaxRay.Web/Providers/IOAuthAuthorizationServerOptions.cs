using Microsoft.Owin.Security.OAuth;

namespace TaxRay.Web.Providers
{
    public interface IOAuthAuthorizationServerOptions
    {
        OAuthAuthorizationServerOptions GetOptions();
    };
}