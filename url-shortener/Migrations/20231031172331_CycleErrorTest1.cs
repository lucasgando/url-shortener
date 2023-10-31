using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace url_shortener.Migrations
{
    /// <inheritdoc />
    public partial class CycleErrorTest1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Urls",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShortUrl",
                value: "MVtGzxxI");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Urls",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShortUrl",
                value: "NcCzPQlk");
        }
    }
}
