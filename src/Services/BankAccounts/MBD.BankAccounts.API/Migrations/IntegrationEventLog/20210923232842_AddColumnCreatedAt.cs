using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MBD.BankAccounts.API.Migrations.IntegrationEventLog
{
    public partial class AddColumnCreatedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "integration_event_logs",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "integration_event_logs");
        }
    }
}
