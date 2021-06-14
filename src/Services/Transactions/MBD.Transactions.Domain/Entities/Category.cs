using System;
using System.Collections.Generic;
using MBD.Core;
using MBD.Core.Entities;
using MBD.Core.Enumerations;
using MBD.Transactions.Domain.Enumerations;

namespace MBD.Transactions.Domain.Entities
{
    public class Category : BaseEntity, IAggregateRoot
    {
        private readonly List<Category> _subCategories = new();

        public Guid UserId { get; private set; }
        public Guid? ParentCategoryId { get; private set; }
        public string Name { get; private set; }
        public TransactionType Type { get; private set; }
        public Status Status { get; private set; }

        public IReadOnlyList<Category> SubCategories => _subCategories.AsReadOnly();

        public Category(Guid userId, string name, TransactionType type)
        {
            Assertions.IsNotEmpty(userId, "Id de usuário inválido.");
            Assertions.IsNotNullOrEmpty(name, "É necessário informar um nome.");
            Assertions.HasMaxLength(name, 100, "O nome deve conter no máximo 100 caracteres.");

            UserId = userId;
            Name = name;
            Type = type;
            Status = Status.Active;
        }

        /// <summary>
        /// Construtor para criação de subcategorias.
        /// </summary>
        private Category(Guid userId, Guid parentCategoryId, string name, TransactionType type)
            : this(userId, name, type)
        {
            Assertions.IsNotEmpty(parentCategoryId, "Id de categoria pai inválido.");

            ParentCategoryId = parentCategoryId;
        }

        public void AddSubCategory(string name)
        {
            _subCategories.Add(new Category(UserId, Id, name, Type));
        }
    }
}