using System;

namespace MBD.BankAccounts.Application.IntegrationEvents
{
    public class TransactionValueChangedIntegrationEvent
    {
        public Guid Id { get; init; }
        public decimal NewValue { get; init; }
        public decimal OldValue { get; init; }
    }
}