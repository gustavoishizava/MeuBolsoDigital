using System;
using MBD.Core.Enumerations;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class CategoryCreatedDomainEvent : DomainEvent
    {
        public Guid Id { get; private init; }
        public Guid TenantId { get; private init; }
        public Guid? ParentCategoryId { get; private init; }
        public string Name { get; private init; }
        public TransactionType Type { get; private init; }
        public Status Status { get; private init; }

        public CategoryCreatedDomainEvent(Category category)
        {
            Id = category.Id;
            TenantId = category.Id;
            ParentCategoryId = category.ParentCategoryId;
            Name = category.Name;
            Type = category.Type;
            Status = category.Status;
        }
    }
}