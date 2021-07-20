using Microsoft.EntityFrameworkCore.Migrations;

namespace SinsensApp.Migrations
{
    public partial class RechangeAccountCurrency2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletCurrencyRates_WalletCurrencies_Code",
                table: "WalletCurrencyRates");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyRateCode",
                table: "WalletCurrencies",
                type: "varchar(32) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletCurrencies_CurrencyRateCode",
                table: "WalletCurrencies",
                column: "CurrencyRateCode");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletCurrencies_WalletCurrencyRates_CurrencyRateCode",
                table: "WalletCurrencies",
                column: "CurrencyRateCode",
                principalTable: "WalletCurrencyRates",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletCurrencies_WalletCurrencyRates_CurrencyRateCode",
                table: "WalletCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_WalletCurrencies_CurrencyRateCode",
                table: "WalletCurrencies");

            migrationBuilder.DropColumn(
                name: "CurrencyRateCode",
                table: "WalletCurrencies");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletCurrencyRates_WalletCurrencies_Code",
                table: "WalletCurrencyRates",
                column: "Code",
                principalTable: "WalletCurrencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
