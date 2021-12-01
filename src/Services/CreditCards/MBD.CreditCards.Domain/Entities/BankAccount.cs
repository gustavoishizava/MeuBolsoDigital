using System;

namespace MBD.CreditCards.Domain.Entities
{
    public class BankAccount
    {
        public Guid Id { get; private init; }
        public Guid TenantId { get; private init; }
        public string Description { get; private init; }

        public BankAccount(Guid id, Guid tenantId, string description)
        {
            Id = id;
            TenantId = tenantId;
            Description = description;
        }
    }
}