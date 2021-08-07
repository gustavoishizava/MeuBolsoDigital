using System;
using MBD.Transactions.Domain.Enumerations;
using MongoDB.Bson.Serialization.Attributes;

namespace MBD.Transactions.Application.Response.Models
{
    public class TransactionModel
    {
        [BsonId]
        public string Id { get; set; }
        public BankAccountModel BankAccount { get; set; }
        public CategoryModel Category { get; set; }
        public DateTime ReferenceDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public TransactionStatus Status { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }

        public TransactionModel(Guid id, BankAccountModel bankAccount, CategoryModel category, DateTime referenceDate, DateTime dueDate, DateTime? paymentDate, TransactionStatus status, decimal value, string description)
        {
            Id = id.ToString();
            BankAccount = bankAccount;
            Category = category;
            ReferenceDate = referenceDate;
            DueDate = dueDate;
            PaymentDate = paymentDate;
            Status = status;
            Value = value;
            Description = description;
        }
    }
}