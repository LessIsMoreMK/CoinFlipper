using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoinFlipper.Tracer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FearAndGreedIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FearAndGreed",
                columns: table => new
                {
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Value = table.Column<int>(type: "integer", precision: 0, nullable: false),
                    Classification = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FearAndGreed", x => x.DateTime);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FearAndGreed_DateTime",
                table: "FearAndGreed",
                column: "DateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FearAndGreed");
        }
    }
}
