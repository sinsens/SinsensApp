using Microsoft.EntityFrameworkCore.Migrations;

namespace SinsensApp.Migrations
{
    public partial class RechangeAccountCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletAccounts_WalletCurrencies_CurrencyCode",
                table: "WalletAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "WalletAccounts",
                type: "varchar(32) CHARACTER SET utf8mb4",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(32) CHARACTER SET utf8mb4",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletAccounts_WalletCurrencies_CurrencyCode",
                table: "WalletAccounts",
                column: "CurrencyCode",
                principalTable: "WalletCurrencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletAccounts_WalletCurrencies_CurrencyCode",
                table: "WalletAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "WalletAccounts",
                type: "varchar(32) CHARACTER SET utf8mb4",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(32) CHARACTER SET utf8mb4",
                oldMaxLength: 32);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletAccounts_WalletCurrencies_CurrencyCode",
                table: "WalletAccounts",
                column: "CurrencyCode",
                principalTable: "WalletCurrencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
