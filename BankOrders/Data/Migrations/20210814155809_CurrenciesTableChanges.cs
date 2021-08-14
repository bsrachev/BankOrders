using Microsoft.EntityFrameworkCore.Migrations;

namespace BankOrders.Data.Migrations
{
    public partial class CurrenciesTableChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_Currencies_CurrencyId",
                table: "Details");

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Currencies_CurrencyId",
                table: "Details",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_Currencies_CurrencyId",
                table: "Details");

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Currencies_CurrencyId",
                table: "Details",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
