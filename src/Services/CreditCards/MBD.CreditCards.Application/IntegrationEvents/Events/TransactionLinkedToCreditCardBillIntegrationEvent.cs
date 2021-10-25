using System;
using MediatR;

namespace MBD.CreditCards.Application.IntegrationEvents.Events
{
    public class TransactionLinkedToCreditCardBillIntegrationEvent : INotification
    {
        public Guid Id { get; init; }
        public Guid BankAccountId { get; init; }
        public Guid CreditCardBillId { get; init; }
        public DateTime CreatedAt { get; init; }
        public decimal Value { get; init; }
    }
}