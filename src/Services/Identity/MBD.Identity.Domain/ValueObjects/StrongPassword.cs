using System;
using System.Text.RegularExpressions;
using MBD.Core.DomainObjects;
using MBD.Identity.Domain.Interfaces.Services;

namespace MBD.Identity.Domain.ValueObjects
{
    /// <summary>
    /// Deve conter ao menos 1 dígito;
    /// Deve conter ao menos 1 letra minúscula;
    /// Deve conter ao menos 1 letra maiúscula;
    /// Deve conter ao menos 1 caracter especial($*&@#);
    /// Deve conter ao menos 8 caracteres.
    /// </summary>
    public class StrongPassword
    {
        public string PasswordHash { get; private set; }

        public StrongPassword(string password, IHashService hashService)
        {
            if (hashService == null)
                throw new ArgumentNullException("Hash service cannot be null.");

            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password cannot be null or empty.");            
            
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$");
            if (!passwordRegex.IsMatch(password))
                throw new DomainException("Weak password.");

            PasswordHash = hashService.Create(password);
        }
    }
}