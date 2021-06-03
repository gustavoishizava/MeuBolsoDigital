using MBD.Core.Entities;
using MBD.Identity.Domain.ValueObjects;

namespace MBD.Identity.Domain.Entities
{
    public class User: BaseEntity
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public string Password { get; private set; }

        public User(string name, string email, string password)
        {
            Name = name;
            SetEmail(email);
            Password = password;
        }

        public void SetEmail(string email)
        {
            Email = new Email(email);
        }
    }
}