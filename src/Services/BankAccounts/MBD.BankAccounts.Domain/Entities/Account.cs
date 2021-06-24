using System;
using System.Collections.Generic;
using System.Linq;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.Core;
using MBD.Core.Entities;
using MBD.Core.Enumerations;

namespace MBD.BankAccounts.Domain.Entities
{
    public class Account : BaseEntity, IAggregateRoot
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();

        public Guid UserId { get; private set; }
        public string Description { get; private set; }
        public decimal InitialBalance { get; private set; }
        public AccountType Type { get; private set; }
        public Status Status { get; private set; }

        #region Navigation
        
        public decimal Balance => InitialBalance + _transactions.Sum(x => x.Type == TransactionType.Income ? x.Value : x.Value * -1);

        #endregion

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

        #region Transactions

        public void AddTransaction(Guid transactionId, DateTime createdAt, decimal value, TransactionType type)
        {
            Assertions.IsFalse(ExistingTransaction(transactionId), $"Transação já existente. Id='{transactionId}'.");

            _transactions.Add(new Transaction(transactionId, Id, createdAt, value, type));
        }

        public Transaction GetTransaction(Guid transactionId)
        {
            return _transactions.Find(x => x.Id == transactionId);
        }

        public bool ExistingTransaction(Guid transactionId)
        {
            return _transactions.Any(x => x.Id == transactionId);
        }

        #endregion
    }
}