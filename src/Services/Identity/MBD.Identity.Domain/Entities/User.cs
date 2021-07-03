using MBD.Core;
using MBD.Core.Entities;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Domain.ValueObjects;

namespace MBD.Identity.Domain.Entities
{
    public class User : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public StrongPassword Password { get; private set; }

        public User(string name, string email, string password, IHashService hashService)
        {
            Assertions.IsNotNullOrEmpty(name, "O nome não pode estar vazio ou nulo.");
            Assertions.HasMaxLength(name, 100, "O nome não pode conter mais que 100 caracteres.");

            Name = name;
            SetEmail(email);
            SetPassword(password, hashService);
        }

        #region EF
        protected User() { }
        #endregion

        #region User        

        public void SetEmail(string email)
        {
            Email = new Email(email);
        }

        public void SetPassword(string password, IHashService hashService)
        {
            Password = new StrongPassword(password, hashService);
        }

        #endregion

        #region Refresh token

        public RefreshToken CreateRefreshToken(int expiresIn = 3600)
        {
            return new RefreshToken(Id, expiresIn);
        }

        #endregion
    }
}