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
            var invalidTenantId = Guid.Empty;
            var invalidName = new String('a', 101);

            // Act && Assert
            Assert.Throws<DomainException>(() => new Category(invalidTenantId, "Categoria", TransactionType.Income));
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
            var tenantId = Guid.NewGuid();

            // Act
            var category = new Category(tenantId, name, type);

            // Assert
            Assert.Equal(tenantId, category.TenantId);
            Assert.Equal(name, category.Name);
            Assert.Equal(type, category.Type);
            Assert.Equal(Status.Active, category.Status);
            Assert.Null(category.ParentCategoryId);
        }

        [Theory(DisplayName = "Atualizar uma categoria existente deve retornar sucesso.")]
        [InlineData("Estudo", true)]
        [InlineData("Telefone", false)]
        public void ValidCategory_UpdateCategory_ReturnSuccess(string name, bool active)
        {
            // Arrange
            var category = new Category(Guid.NewGuid(), "Categoria", TransactionType.Income);
            var statusExcepted = active ? Status.Active : Status.Inactive;

            // Act
            category.SetName(name);
            if (active)
                category.Activate();
            else
                category.Deactivate();

            // Arrange
            Assert.Equal(name, category.Name);
            Assert.Equal(statusExcepted, category.Status);
        }

        [Theory(DisplayName = "Adicionar subcategoria a uma categoria válida deve retornar sucesso.")]
        [InlineData(TransactionType.Income)]
        [InlineData(TransactionType.Expense)]
        public void ValidCategory_AddNewSubCategory_ReturnSuccess(TransactionType type)
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var category = new Category(tenantId, "Categoria pai", type);
            Category subCategory = null;

            // Act
            subCategory = category.AddSubCategory("Categoria filha");

            // Assert
            Assert.NotNull(subCategory);
            Assert.NotEmpty(category.SubCategories);
            Assert.Equal(type, subCategory.Type);
            Assert.Single(category.SubCategories);
        }

        [Fact(DisplayName = "Adicionar subcategoria à uma subcategoria deve retornar Domain Exception.")]
        public void ValidSubCategory_AddNewSubCategoryToSubCategory_ReturnDomainException()
        {
            // Arrange
            var category = new Category(Guid.NewGuid(), "Categoria pai", TransactionType.Income);
            var subCategory = category.AddSubCategory("Subcategoria");

            // Act && Assert
            Assert.Throws<DomainException>(() => subCategory.AddSubCategory("Nova Subcategoria"));
        }
    }
}