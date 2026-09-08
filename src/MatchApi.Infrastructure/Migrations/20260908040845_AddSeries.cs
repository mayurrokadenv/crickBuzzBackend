using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Fixtures",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Series_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeriesTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeriesName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriesTeams_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_SeriesId",
                table: "Fixtures",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_SportId",
                table: "Series",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesTeams_SeriesId",
                table: "SeriesTeams",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesTeams_TeamId",
                table: "SeriesTeams",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Series_SeriesId",
                table: "Fixtures",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Series_SeriesId",
                table: "Fixtures");

            migrationBuilder.DropTable(
                name: "SeriesTeams");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_SeriesId",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Fixtures");
        }
    }
}
