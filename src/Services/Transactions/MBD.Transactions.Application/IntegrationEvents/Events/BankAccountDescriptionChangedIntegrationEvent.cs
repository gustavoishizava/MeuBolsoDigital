using System;
using MediatR;

namespace MBD.Transactions.Application.IntegrationEvents.Events
{
    public class BankAccountDescriptionChangedIntegrationEvent : INotification
    {
        public Guid Id { get; init; }
        public string NewDescription { get; init; }
        public string OldDescription { get; init; }

        public BankAccountDescriptionChangedIntegrationEvent(Guid id, string newDescription, string oldDescription)
        {
            Id = id;
            NewDescription = newDescription;
            OldDescription = oldDescription;
        }
    }
}