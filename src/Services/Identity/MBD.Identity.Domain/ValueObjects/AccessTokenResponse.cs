using System;

namespace MBD.Identity.Domain.ValueObjects
{
    public sealed class AccessTokenResponse
    {
        public string Error { get; private set; }
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        public int ExpiresIn { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public bool HasErrors => !string.IsNullOrEmpty(Error);

        private AccessTokenResponse(string error)
        {
            Error = error;
        }

        private AccessTokenResponse(string accessToken, string refreshToken, int expiresIn, DateTime createdAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
            CreatedAt = createdAt;
        }

        public static class AccessTokenResponseFactory
        {
            public static AccessTokenResponse Success(string accessToken, string refreshToken, int expiresIn, DateTime createdAt)
            {
                return new AccessTokenResponse(accessToken, refreshToken, expiresIn, createdAt);
            }

            public static AccessTokenResponse Fail(string error)
            {
                return new AccessTokenResponse(error);
            }
        }
    }
}