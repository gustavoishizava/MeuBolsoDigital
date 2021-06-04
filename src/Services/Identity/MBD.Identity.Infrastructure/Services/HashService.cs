using System;
using MBD.Identity.Domain.Interfaces.Services;

namespace MBD.Identity.Infrastructure.Services
{
    public sealed class HashService : IHashService
    {
        public string Create(string input)
        {
            if(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be null or empty.");

            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public bool IsMatch(string input, string hash)
        {
            if(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be null or empty.");
            if(string.IsNullOrEmpty(hash) || string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Hash cannot be null or empty.");

            return BCrypt.Net.BCrypt.Verify(input, hash);
        }
    }
}