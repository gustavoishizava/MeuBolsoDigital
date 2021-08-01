using System;
using MBD.Core.Enumerations;
using MBD.Transactions.Domain.Enumerations;

namespace MBD.Transactions.Application.Response
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public TransactionType Type { get; set; }
        public Status Status { get; set; }
    }
}