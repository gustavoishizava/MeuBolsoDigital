using System;
using System.Collections.Generic;
using System.Linq;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.BankAccounts.Domain.Resources;
using MBD.Core;
using MBD.Core.Entities;
using MBD.Core.Enumerations;
using MBD.Core.Extensions;

namespace MBD.BankAccounts.Domain.Entities
{
    public class Account : BaseEntity, IAggregateRoot, ITenant
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();

        public Guid TenantId { get; private set; }
        public string Description { get; private set; }
        public decimal InitialBalance { get; private set; }
        public AccountType Type { get; private set; }
        public Status Status { get; private set; }

        #region Navigation

        public decimal Balance => InitialBalance + _transactions.Sum(x => x.Type == TransactionType.Income ? x.Value : x.Value * -1);

        #endregion

        public Account(Guid tenantId, string description, decimal initialBalance, AccountType type)
        {
            Assertions.IsGreaterOrEqualsThan(initialBalance, 0, ResourceCodes.Account.InitialValueMinValue.GetResource());

            TenantId = tenantId;
            SetDescription(description);
            InitialBalance = initialBalance;
            SetType(type);
            Activate();
        }

        #region Account

        public void SetDescription(string description)
        {
            Assertions.IsNotNullOrEmpty(description, ResourceCodes.Account.DescriptionEmpty.GetResource());
            Assertions.HasMaxLength(description, 150, ResourceCodes.Account.DescriptionMaxLength.GetResource());

            Description = description;
        }

        public void SetType(AccountType type)
        {
            Type = type;
        }

        public void Activate()
        {
            Status = Status.Active;
        }

        public void Deactivate()
        {
            Status = Status.Inactive;
        }

        #endregion

        #region Transactions

        public void AddTransaction(Guid transactionId, DateTime createdAt, decimal value, TransactionType type)
        {
            Assertions.IsFalse(ExistingTransaction(transactionId), String.Format(ResourceCodes.Account.DuplicateTransaction.GetResource(), transactionId));

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