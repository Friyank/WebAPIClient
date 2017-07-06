using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApiHelper
{
    public class AuthenitcateCaller : DelegatingHandler
    {
        private AuthenticationHeaderValue authHeader;
        public AuthenitcateCaller(string serviceUrl, string clientId, HttpMessageHandler innerHandler, string UserName, string Password)
            : base(innerHandler)
        {
            // Obtain the Azure Active Directory Authentication Library (ADAL) authentication context.
            AuthenticationParameters ap = AuthenticationParameters.CreateFromResourceUrlAsync(
                    new Uri(serviceUrl + "api/data/")).Result;
            AuthenticationContext authContext = new AuthenticationContext(ap.Authority, false);

            //Note that an Azure AD access token has finite lifetime, default expiration is 60 minutes.
            Task<AuthenticationResult> authResult = authContext.AcquireTokenAsync(serviceUrl, clientId, new UserPasswordCredential(UserName, Password));
            authHeader = new AuthenticationHeaderValue("Bearer", authResult.Result.AccessToken);

        }

        protected override Task<HttpResponseMessage> SendAsync(
                 HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Authorization = authHeader;
            return base.SendAsync(request, cancellationToken);
        }
    }
}
