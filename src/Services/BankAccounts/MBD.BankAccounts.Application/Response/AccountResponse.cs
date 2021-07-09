using System;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.Core.Enumerations;

namespace MBD.BankAccounts.Application.Response
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        public AccountType Type { get; set; }
        public Status Status { get; set; }
    }
}