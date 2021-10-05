using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MBD.Transactions.API.Migrations
{
    public partial class AddCreditCardBillId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "credit_card_bill_id",
                table: "transactions",
                type: "uuid",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "credit_card_bill_id",
                table: "transactions");
        }
    }
}
