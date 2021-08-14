using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankOrders.Data.Migrations
{
    public partial class BankOrdersTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserApproveId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPostingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserApprovePostingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RefNumber = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    System = table.Column<int>(type: "int", nullable: false),
                    UserCreateId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimesUsed = table.Column<int>(type: "int", nullable: false),
                    RefNumber = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    System = table.Column<int>(type: "int", nullable: false),
                    UserCreateId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    Branch = table.Column<int>(type: "int", nullable: false),
                    CostCenter = table.Column<int>(type: "int", nullable: false),
                    Project = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AccountType = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    Sum = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    SumBGN = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    AccountingNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Details_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Details_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Details_OrderId",
                table: "Details",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_TemplateId",
                table: "Details",
                column: "TemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}
