using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MBD.Identity.Domain.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;

namespace MBD.Identity.Infrastructure.Services
{
    public sealed class JwtService : IJwtService
    {
        private readonly SymmetricSecurityKey _symmetricSecurityKey;

        public JwtService(SymmetricSecurityKey symmetricSecurityKey)
        {
            _symmetricSecurityKey = symmetricSecurityKey;
        }

        public string Generate(string issuer, string audience, DateTime createdAt, DateTime expiresIn, IEnumerable<Claim> claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Expires = expiresIn,
                NotBefore = createdAt,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }        
    }
}