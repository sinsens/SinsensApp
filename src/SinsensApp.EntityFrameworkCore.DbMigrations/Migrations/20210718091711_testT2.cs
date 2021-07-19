using Microsoft.EntityFrameworkCore.Migrations;

namespace SinsensApp.Migrations
{
    public partial class testT2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ToCode",
                table: "WalletCurrencyRateDetails",
                type: "varchar(32) CHARACTER SET utf8mb4",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromCode",
                table: "WalletCurrencyRateDetails",
                type: "varchar(32) CHARACTER SET utf8mb4",
                maxLength: 32,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromCode",
                table: "WalletCurrencyRateDetails");

            migrationBuilder.AlterColumn<string>(
                name: "ToCode",
                table: "WalletCurrencyRateDetails",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(32) CHARACTER SET utf8mb4",
                oldMaxLength: 32);
        }
    }
}
