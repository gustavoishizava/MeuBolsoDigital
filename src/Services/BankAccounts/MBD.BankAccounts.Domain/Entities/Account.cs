using System;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.Core;
using MBD.Core.Entities;
using MBD.Core.Enumerations;

namespace MBD.BankAccounts.Domain.Entities
{
    public class Account : BaseEntity
    {        
        public Guid UserId { get; private set; }
        public string Description { get; private set; }
        public decimal InitialBalance { get; private set; }
        public AccountType Type { get; private set; }
        public Status Status { get; private set; }

        public Account(Guid userId, string description, decimal initialBalance, AccountType type)
        {
            Assertions.IsNotNullOrEmpty(description, "A descrição deve ser informada.");
            Assertions.HasMaxLength(description, 150, "A descrição deve conter no máximo 150 caracteres.");
            Assertions.IsGreaterOrEqualsThan(initialBalance, 0, "O saldo inicial não pode ser inferiror a R$0,00.");

            UserId = userId;
            Description = description;
            InitialBalance = initialBalance;
            Type = type;
            Status = Status.Active;
        }
    }
}