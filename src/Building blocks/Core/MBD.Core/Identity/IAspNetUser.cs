using System;
namespace MBD.Core.Identity
{
    public interface IAspNetUser
    {
        Guid UserId { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
    }
}