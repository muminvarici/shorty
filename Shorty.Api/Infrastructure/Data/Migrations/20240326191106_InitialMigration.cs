using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shorty.Api.Infrastructure.Data.Migrations
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
                    IsSingleUsage = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrlUsageHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UrlId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlUsageHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlUsageHistories_UrlDetails_UrlId",
                        column: x => x.UrlId,
                        principalTable: "UrlDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlDetails_Code",
                table: "UrlDetails",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UrlUsageHistories_UrlId",
                table: "UrlUsageHistories",
                column: "UrlId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlUsageHistories");

            migrationBuilder.DropTable(
                name: "UrlDetails");
        }
    }
}
