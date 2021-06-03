using System.Text.RegularExpressions;
using System;
namespace MBD.Identity.Domain.ValueObjects
{
    public class Email
    {
        public string Address { get; private set; }
        public string NormalizedAddress { get; private set; }

        public Email(string address)
        {
            if(string.IsNullOrEmpty(address) || string.IsNullOrWhiteSpace(address))
                throw new ArgumentNullException("Email cannot be null or empty.");

            var emailRegex = new Regex(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");
            if(!emailRegex.IsMatch(address.ToLower()))
                throw new ArgumentException("Invalid email.");

            Address = address.ToLower();
            NormalizedAddress = Address.ToUpper();
        }
    }
}