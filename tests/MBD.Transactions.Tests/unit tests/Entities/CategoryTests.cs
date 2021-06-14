using System.Linq;
using System;
using MBD.Core.DomainObjects;
using MBD.Core.Enumerations;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using Xunit;

namespace MBD.Transactions.Tests.unit_tests.Entities
{
    public class CategoryTests
    {
        [Fact(DisplayName = "Criar categoria com parâmetros inválido deve retornar Domain Exception.")]
        public void InvalidParameters_NewCategory_ReturnDomainException()
        {
            // Arrange
            var invalidUserId = Guid.Empty;
            var invalidName = new String('a', 101);

            // Act && Assert
            Assert.Throws<DomainException>(() => new Category(invalidUserId, "Categoria", TransactionType.Income));
            Assert.Throws<DomainException>(() => new Category(Guid.NewGuid(), invalidName, TransactionType.Expense));
        }

        [Theory(DisplayName = "Criar categoria com parâmetros válidos deve retornar sucesso.")]
        [InlineData("Restaurante", TransactionType.Expense)]
        [InlineData("Salário", TransactionType.Income)]
        [InlineData("Automóvel", TransactionType.Expense)]
        [InlineData("Freelances", TransactionType.Income)]
        public void ValidParameters_NewCategory_ReturnSuccess(string name, TransactionType type)
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var category = new Category(userId, name, type);

            // Assert
            Assert.Equal(userId, category.UserId);
            Assert.Equal(name, category.Name);
            Assert.Equal(type, category.Type);
            Assert.Equal(Status.Active, category.Status);
            Assert.Null(category.ParentCategoryId);
        }

        [Theory(DisplayName = "Adicionar subcategoria a uma categoria válida deve retornar sucesso.")]
        [InlineData(TransactionType.Income)]
        [InlineData(TransactionType.Expense)]
        public void ValidCategory_AddNewSubCategory_ReturnSuccess(TransactionType type)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var category = new Category(userId, "Categoria pai", type);
            Category subCategory = null;

            // Act
            category.AddSubCategory("Categoria filha");
            subCategory = category.SubCategories.FirstOrDefault();

            // Assert
            Assert.NotNull(subCategory);
            Assert.NotEmpty(category.SubCategories);
            Assert.Equal(type, subCategory.Type);
        }
    }
}