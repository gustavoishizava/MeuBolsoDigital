using System.Collections.Generic;
using MBD.Core.Entities;
using MBD.Transactions.Domain.Events;

namespace MBD.Transactions.Domain.Entities.Common
{
    public abstract class BaseEntityWithEvent : BaseEntity
    {
        private List<DomainEvent> _events;
        public IReadOnlyList<DomainEvent> Events => _events?.AsReadOnly();

        protected void AddDomainEvent(DomainEvent @event)
        {
            _events = _events ?? new();
            _events.Add(@event);
        }

        protected void RemoveDomainEvent(DomainEvent @event)
        {
            _events?.Remove(@event);
        }

        protected void ClearDomainEvents()
        {
            _events?.Clear();
        }
    }
}