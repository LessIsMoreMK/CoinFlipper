using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoinFlipper.Tracer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CoinAndCoinData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    CoinGeckoId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coin", x => x.Id);
                    table.UniqueConstraint("AK_Coin_CoinGeckoId", x => x.CoinGeckoId);
                    table.UniqueConstraint("AK_Coin_Symbol", x => x.Symbol);
                });

            migrationBuilder.CreateTable(
                name: "CoinData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoinId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Volume24h = table.Column<decimal>(type: "numeric", nullable: false),
                    Volume = table.Column<decimal>(type: "numeric", nullable: false),
                    MarketCap = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinData_Coin_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coin_CoinGeckoId",
                table: "Coin",
                column: "CoinGeckoId");

            migrationBuilder.CreateIndex(
                name: "IX_Coin_Id",
                table: "Coin",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Coin_Symbol",
                table: "Coin",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_CoinData_CoinId",
                table: "CoinData",
                column: "CoinId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinData");

            migrationBuilder.DropTable(
                name: "Coin");
        }
    }
}
