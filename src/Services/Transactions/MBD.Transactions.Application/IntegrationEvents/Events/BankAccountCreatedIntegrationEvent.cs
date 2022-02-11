using System;
using MediatR;

namespace MBD.Transactions.Application.IntegrationEvents.Events
{
    public class BankAccountCreatedIntegrationEvent : INotification
    {
        public Guid Id { get; init; }
        public Guid TenantId { get; init; }
        public string Description { get; init; }

        public BankAccountCreatedIntegrationEvent(Guid id, Guid tenantId, string description)
        {
            Id = id;
            TenantId = tenantId;
            Description = description;
        }
    }
}