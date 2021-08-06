using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MBD.Core.Identity;
using Microsoft.AspNetCore.Authentication;

namespace MBD.CreditCards.API.Configuration.HttpClient
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IAspNetUser _aspNetUser;

        public HttpClientAuthorizationDelegatingHandler(IAspNetUser aspNetUser)
        {
            _aspNetUser = aspNetUser;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _aspNetUser.GetHttpContext().Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && !request.Headers.Any(x => x.Key == "Authorization"))
                request.Headers.Add("Authorization", new List<string>() { authorizationHeader });

            var accessToken = _aspNetUser.GetHttpContext().GetTokenAsync("access_token").GetAwaiter().GetResult();
            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return base.SendAsync(request, cancellationToken);
        }
    }
}