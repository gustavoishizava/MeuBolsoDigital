using System;
using MediatR;

namespace MBD.CreditCards.Application.IntegrationEvents.Events
{
    public class BankAccountCreatedIntegrationEvent : INotification
    {
        public Guid Id { get; init; }
        public Guid TenantId { get; init; }
        public string Description { get; init; }
    }
}