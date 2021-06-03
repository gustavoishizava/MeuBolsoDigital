using System.Security.Claims;
using System;
using System.Collections.Generic;

namespace MBD.Identity.Domain.Interfaces.Services
{
    public interface IJwtService
    {
        string Generate(string issuer, string audience, DateTime createdAt, DateTime expiresAt, IEnumerable<Claim> claims);
    }
}