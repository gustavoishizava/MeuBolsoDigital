using System;
using MediatR;

namespace MBD.BankAccounts.Application.IntegrationEvents
{
    public class BankAccountDescriptionChangedIntegrationEvent : INotification
    {
        public Guid Id { get; init; }
        public string OldDescription { get; init; }
        public string NewDescription { get; init; }

        public BankAccountDescriptionChangedIntegrationEvent(Guid id, string oldDescription, string newDescription)
        {
            Id = id;
            OldDescription = oldDescription;
            NewDescription = newDescription;
        }
    }
}