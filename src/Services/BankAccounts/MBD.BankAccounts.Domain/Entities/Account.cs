using System;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.Core.Entities;
using MBD.Core.Enumerations;

namespace MBD.BankAccounts.Domain.Entities
{
    public class Account : BaseEntity
    {        
        public Guid UserId { get; private set; }
        public string Description { get; private set; }
        public AccountType Type { get; private set; }
        public Status Status { get; private set; }

        public Account(Guid userId, string description, AccountType type)
        {
            UserId = userId;
            Description = description;
            Type = type;
            Status = Status.Active;
        }
    }
}