using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MBD.CreditCards.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "credit_cards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    closing_day = table.Column<int>(type: "integer", nullable: false),
                    day_of_payment = table.Column<int>(type: "integer", nullable: false),
                    limit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    brand = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_credit_cards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "credit_card_bills",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    credit_card_id = table.Column<Guid>(type: "uuid", nullable: false),
                    closes_in = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    reference_month = table.Column<int>(type: "integer", nullable: true),
                    reference_year = table.Column<int>(type: "integer", nullable: true),
                    credit_card_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_credit_card_bills", x => x.id);
                    table.ForeignKey(
                        name: "fk_credit_card_bills_credit_cards_credit_card_id",
                        column: x => x.credit_card_id,
                        principalTable: "credit_cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_credit_card_bills_credit_cards_credit_card_id1",
                        column: x => x.credit_card_id1,
                        principalTable: "credit_cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    credit_card_bill_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    value = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    credit_card_bill_id1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_transactions_credit_card_bills_credit_card_bill_id",
                        column: x => x.credit_card_bill_id,
                        principalTable: "credit_card_bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transactions_credit_card_bills_credit_card_bill_id1",
                        column: x => x.credit_card_bill_id1,
                        principalTable: "credit_card_bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_credit_card_bills_credit_card_id",
                table: "credit_card_bills",
                column: "credit_card_id");

            migrationBuilder.CreateIndex(
                name: "ix_credit_card_bills_credit_card_id1",
                table: "credit_card_bills",
                column: "credit_card_id1");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_credit_card_bill_id",
                table: "transactions",
                column: "credit_card_bill_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_credit_card_bill_id1",
                table: "transactions",
                column: "credit_card_bill_id1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "credit_card_bills");

            migrationBuilder.DropTable(
                name: "credit_cards");
        }
    }
}
