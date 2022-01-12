using MBD.Transactions.Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace MBD.Transactions.Application.Response.Models
{
    public class CategoryModel
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }

        public CategoryModel(Category category)
        {
            Id = category.Id.ToString();
            Name = category.Name;
        }
    }
}