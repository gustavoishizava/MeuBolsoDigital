using MBD.Transactions.Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace MBD.Transactions.Application.Response.Models
{
    public class BankAccountModel
    {
        [BsonId]
        public string Id { get; set; }
        public string Description { get; set; }

        public BankAccountModel(BankAccount bankAccount)
        {
            Id = bankAccount.Id.ToString();
            Description = bankAccount.Description;
        }
    }
}