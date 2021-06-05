using System;

namespace MBD.Identity.Domain.DTO
{
    public sealed class AccessTokenResponse
    {
        public AccessTokenResponse(string error)
        {
            Error = error;
        }
        
        public AccessTokenResponse(string accessToken, string refreshToken, int expiresIn, DateTime createdAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
            CreatedAt = createdAt;
        }

        public string Error { get; private set; }
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        public int ExpiresIn { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public bool HasErrors => !string.IsNullOrEmpty(Error);
    }
}