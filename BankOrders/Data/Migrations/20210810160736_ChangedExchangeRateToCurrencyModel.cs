using Microsoft.EntityFrameworkCore.Migrations;

namespace BankOrders.Data.Migrations
{
    public partial class ChangedExchangeRateToCurrencyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Currencies");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Details",
                newName: "CurrencyId");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Currencies",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Details_CurrencyId",
                table: "Details",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Currencies_CurrencyId",
                table: "Details",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_Currencies_CurrencyId",
                table: "Details");

            migrationBuilder.DropIndex(
                name: "IX_Details_CurrencyId",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Currencies");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "Details",
                newName: "Currency");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Currencies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
