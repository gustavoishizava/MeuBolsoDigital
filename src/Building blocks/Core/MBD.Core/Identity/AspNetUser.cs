using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MBD.Core.Identity
{
    public class AspNetUser : IAspNetUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId =>
            IsAuthenticated
            ? Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
            : Guid.Empty;

        public string Email =>
            IsAuthenticated
            ? _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value
            : string.Empty;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        public HttpContext GetHttpContext()
        {
            return _httpContextAccessor.HttpContext;
        }
    }
}