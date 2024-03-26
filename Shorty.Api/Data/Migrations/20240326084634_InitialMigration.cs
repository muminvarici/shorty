using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shorty.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LongUrl = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    ShortUrl = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlDetails_Code",
                table: "UrlDetails",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlDetails");
        }
    }
}
