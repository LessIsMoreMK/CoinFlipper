using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoinFlipper.SwissArmy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sentences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sentence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sentence = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentence", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Sentence",
                columns: new[] { "Id", "Author", "Sentence" },
                values: new object[,]
                {
                    { 1, "Satoshi Nakamoto", "If you don’t believe it or don’t get it, I don’t have the time to try to convince you, sorry." },
                    { 2, "", "Scams always pump the hardest in a risk-on environment." },
                    { 3, "", "A clear mind is a foundation of a good trade." },
                    { 4, "", "Knowledge opens a lot of doors." },
                    { 5, "", "Little of something is greater than all of nothing." },
                    { 6, "", "Learn at your first bull run, retire at second." },
                    { 7, "T. Harv Eker", "How you do anything is how you do everything." },
                    { 8, "", "What you don't use you lose." },
                    { 9, "", "All good things come to those who wait." },
                    { 10, "", "The bull walks up the stairs and the bear jumps out the window." },
                    { 11, "Sheryl Sandberg", "Done is better than perfect." },
                    { 12, "Socrates", "The secret to change is to focus all of your energy not on fighting the old, but building the new." },
                    { 13, "", "Man becomes slave of his own repeated actions." },
                    { 14, "Benjamin Franklin", "An investment in knowledge pays the best interest." },
                    { 15, "Carlos Slim Helu", "With a good perspective on history, we can have a better understanding of the past and present, and thus a clear vision of the future." },
                    { 16, "Mellody Hobson", "The biggest risk of all is not taking one." },
                    { 17, "Peter Lynch", "Know what you own, and know why you own it." },
                    { 18, "Warren Buffett", "Wide diversification is only required when investors do not understand what they are doing." },
                    { 19, "Peter Thiel", "The most contrarian thing of all is not to oppose the crowd but to think for yourself." },
                    { 20, "Aya Laraya", "When you invest, you are buying a day that you don’t have to work." },
                    { 21, "Jim Rohn", "Formal education will make you a living; self-education will make you a fortune." },
                    { 22, "Grant Cardone", "Investing puts money to work. The only reason to save money is to invest it." },
                    { 23, "", "Do not put all your eggs in one basket." },
                    { 24, "Mark Twain", "Courage is note the lack of fear. It is acting in spite of it." },
                    { 25, "Steve Clark", "Do more of what works and less of what doesn’t." },
                    { 26, "Warren Buffett", "Risk comes from not knowing what you are doing." },
                    { 27, "Warren Buffett", "If you cannot control your emotions, you can’t control your money." },
                    { 28, "Marty Schwartz", "Learn to take losses. The most important thing in making money is not letting your losses get out of hand." },
                    { 29, "", "Always check correlations to see bigger picture." },
                    { 30, "", "Price is never \"too high\" or \"too low\"." },
                    { 31, "", "Ping-Pong till you wrong." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sentence");
        }
    }
}
