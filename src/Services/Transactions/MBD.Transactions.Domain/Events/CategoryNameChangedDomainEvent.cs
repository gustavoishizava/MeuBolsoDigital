using System;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class CategoryNameChangedDomainEvent : DomainEvent
    {
        public Guid Id { get; private init; }
        public string NewName { get; private init; }
        public string OldName { get; private init; }

        public CategoryNameChangedDomainEvent(Guid id, string newName, string oldName)
        {
            Id = id;
            NewName = newName;
            OldName = oldName;
        }
    }
}