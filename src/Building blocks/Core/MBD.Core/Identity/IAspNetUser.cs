using System;
using Microsoft.AspNetCore.Http;

namespace MBD.Core.Identity
{
    public interface IAspNetUser
    {
        Guid UserId { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
        HttpContext GetHttpContext();
    }
}