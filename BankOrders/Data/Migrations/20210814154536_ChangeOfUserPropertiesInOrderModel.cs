using Microsoft.EntityFrameworkCore.Migrations;

namespace BankOrders.Data.Migrations
{
    public partial class ChangeOfUserPropertiesInOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserCreateId",
                table: "Templates",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserPostingId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserCreateId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserApprovePostingId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserApproveId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Templates_UserCreateId",
                table: "Templates",
                column: "UserCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserApproveId",
                table: "Orders",
                column: "UserApproveId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserApprovePostingId",
                table: "Orders",
                column: "UserApprovePostingId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserCreateId",
                table: "Orders",
                column: "UserCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserPostingId",
                table: "Orders",
                column: "UserPostingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserApproveId",
                table: "Orders",
                column: "UserApproveId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserApprovePostingId",
                table: "Orders",
                column: "UserApprovePostingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserCreateId",
                table: "Orders",
                column: "UserCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserPostingId",
                table: "Orders",
                column: "UserPostingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_AspNetUsers_UserCreateId",
                table: "Templates",
                column: "UserCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserApproveId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserApprovePostingId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserCreateId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserPostingId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_AspNetUsers_UserCreateId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_UserCreateId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserApproveId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserApprovePostingId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserCreateId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserPostingId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "UserCreateId",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserPostingId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserCreateId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserApprovePostingId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserApproveId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
