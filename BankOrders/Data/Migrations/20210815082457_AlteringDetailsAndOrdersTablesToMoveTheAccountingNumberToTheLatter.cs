using Microsoft.EntityFrameworkCore.Migrations;

namespace BankOrders.Data.Migrations
{
    public partial class AlteringDetailsAndOrdersTablesToMoveTheAccountingNumberToTheLatter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountingNumber",
                table: "Details");

            migrationBuilder.AddColumn<int>(
                name: "PostingNumber",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostingNumber",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "AccountingNumber",
                table: "Details",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
