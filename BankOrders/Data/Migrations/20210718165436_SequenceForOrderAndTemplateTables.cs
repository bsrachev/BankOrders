using Microsoft.EntityFrameworkCore.Migrations;

namespace BankOrders.Data.Migrations
{
    public partial class SequenceForOrderAndTemplateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "OrderRefNumSeq",
                startValue: 10000001L);

            migrationBuilder.CreateSequence<int>(
                name: "TemplateOrderRefNumSeq",
                startValue: 90000001L);

            migrationBuilder.AlterColumn<int>(
                name: "RefNumber",
                table: "Templates",
                type: "int",
                maxLength: 20,
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR TemplateOrderRefNumSeq",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "RefNumber",
                table: "Orders",
                type: "int",
                maxLength: 20,
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR OrderRefNumSeq",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "OrderRefNumSeq");

            migrationBuilder.DropSequence(
                name: "TemplateOrderRefNumSeq");

            migrationBuilder.AlterColumn<int>(
                name: "RefNumber",
                table: "Templates",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20,
                oldDefaultValueSql: "NEXT VALUE FOR TemplateOrderRefNumSeq");

            migrationBuilder.AlterColumn<int>(
                name: "RefNumber",
                table: "Orders",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20,
                oldDefaultValueSql: "NEXT VALUE FOR OrderRefNumSeq");
        }
    }
}
