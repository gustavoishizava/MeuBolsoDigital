using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MBD.Transactions.Application.Response.Models
{
    public class BankAccountModel
    {
        [BsonId]
        public string Id { get; set; }
        public string Description { get; set; }

        public BankAccountModel(Guid id, string description)
        {
            Id = id.ToString();
            Description = description;
        }
    }
}