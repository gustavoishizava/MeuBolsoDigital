using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MBD.Transactions.Application.Response.Models
{
    public class CategoryModel
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }

        public CategoryModel(Guid id, string name)
        {
            Id = id.ToString();
            Name = name;
        }
    }
}