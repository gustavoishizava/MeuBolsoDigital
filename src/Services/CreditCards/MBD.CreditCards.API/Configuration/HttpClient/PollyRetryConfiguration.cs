using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace MBD.CreditCards.API.Configuration.HttpClient
{
    public static class PollyRetryConfiguration
    {
        public static AsyncRetryPolicy<HttpResponseMessage> WaitToRetry()
        {
            var retry = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                });
                
            return retry;
        }
    }
}