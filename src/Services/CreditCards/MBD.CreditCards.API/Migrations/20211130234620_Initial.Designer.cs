// <auto-generated />
using System;
using MBD.CreditCards.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MBD.CreditCards.API.Migrations
{
    [DbContext(typeof(CreditCardContext))]
    [Migration("20211130234620_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MBD.CreditCards.Domain.Entities.BankAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("description");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.HasKey("Id")
                        .HasName("pk_bank_accounts");

                    b.ToTable("bank_accounts", (string)null);
                });

            modelBuilder.Entity("MBD.CreditCards.Domain.Entities.CreditCard", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BankAccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("bank_account_id");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR(20)")
                        .HasColumnName("brand");

                    b.Property<int>("ClosingDay")
                        .HasColumnType("integer")
                        .HasColumnName("closing_day");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("DayOfPayment")
                        .HasColumnType("integer")
                        .HasColumnName("day_of_payment");

                    b.Property<decimal>("Limit")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("limit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("VARCHAR(10)")
                        .HasColumnName("status");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_credit_cards");

                    b.HasIndex("BankAccountId")
                        .HasDatabaseName("ix_credit_cards_bank_account_id");

                    b.ToTable("credit_cards", (string)null);
                });

            modelBuilder.Entity("MBD.CreditCards.Domain.Entities.CreditCardBill", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("ClosesIn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("closes_in");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("CreditCardId")
                        .HasColumnType("uuid")
                        .HasColumnName("credit_card_id");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("due_date");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_credit_card_bills");

                    b.HasIndex("CreditCardId")
                        .HasDatabaseName("ix_credit_card_bills_credit_card_id");

                    b.ToTable("credit_card_bills", (string)null);
                });

            modelBuilder.Entity("MBD.CreditCards.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("CreditCardBillId")
                        .HasColumnType("uuid")
                        .HasColumnName("credit_card_bill_id");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_transactions");

                    b.HasIndex("CreditCardBillId")
                        .HasDatabaseName("ix_transactions_credit_card_bill_id");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("MBD.CreditCards.Domain.Entities.CreditCard", b =>
                {
                    b.HasOne("MBD.CreditCards.Domain.Entities.BankAccount", "BankAccount")
                        .WithMany()
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_credit_cards_bank_accounts_bank_account_id");

                    b.Navigation("BankAccount");
                });

            modelBuilder.Entity("MBD.CreditCards.Domain.Entities.CreditCardBill", b =>
                {
                    b.HasOne("MBD.CreditCards.Domain.Entities.CreditCard", null)
                        .WithMany("Bills")
                        .HasForeignKey("CreditCardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_credit_card_bills_credit_cards_credit_card_id");

                    b.OwnsOne("MBD.CreditCards.Domain.ValueObjects.BillReference", "Reference", b1 =>
                        {
                            b1.Property<Guid>("CreditCardBillId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<int>("Month")
                                .HasColumnType("integer")
                                .HasColumnName("reference_month");

                            b1.Property<int>("Year")
                                .HasColumnType("integer")
                                .HasColumnName("reference_year");

                            b1.HasKey("CreditCardBillId");

                            b1.ToTable("credit_card_bills");

                            b1.WithOwner()
                                .HasForeignKey("CreditCardBillId")
                                .HasConstraintName("fk_credit_card_bills_credit_card_bills_id");
                        });

                    b.Navigation("Reference");
                });

            modelBuilder.Entity("MBD.CreditCards.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("MBD.CreditCards.Domain.Entities.CreditCardBill", null)
                        .WithMany("Transactions")
                        .HasForeignKey("CreditCardBillId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_credit_card_bills_credit_card_bill_id");
                });

            modelBuilder.Entity("MBD.CreditCards.Domain.Entities.CreditCard", b =>
                {
                    b.Navigation("Bills");
                });

            modelBuilder.Entity("MBD.CreditCards.Domain.Entities.CreditCardBill", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
