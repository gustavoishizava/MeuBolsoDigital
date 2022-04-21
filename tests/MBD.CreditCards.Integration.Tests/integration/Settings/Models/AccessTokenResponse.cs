using System;

namespace MBD.CreditCards.Integration.Tests.integration.Settings.Models
{
    public class AccessTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}