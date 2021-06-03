using MBD.Core;
using MBD.Identity.Domain.Interfaces.Services;

namespace MBD.Identity.Infrastructure.Services
{
    public sealed class HashService : IHashService
    {
        public string Create(string input)
        {
            Assertions.IsNotNullOrEmpty(input, "Input cannot be null or empty.");
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public bool IsMatch(string input, string hash)
        {
            Assertions.IsNotNullOrEmpty(input, "Input cannot be null or empty.");
            Assertions.IsNotNullOrEmpty(hash, "Hash cannot be null or empty.");

            return BCrypt.Net.BCrypt.Verify(input, hash);
        }
    }
}