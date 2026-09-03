using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddScorecard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scorecards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FixtureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InningsNo = table.Column<int>(type: "int", nullable: false),
                    BattingTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BowlingTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scorecards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scorecards_Fixtures_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixtures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scorecards_Teams_BattingTeamId",
                        column: x => x.BattingTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scorecards_Teams_BowlingTeamId",
                        column: x => x.BowlingTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BattingFigures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScorecardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Runs = table.Column<int>(type: "int", nullable: false),
                    Balls = table.Column<int>(type: "int", nullable: false),
                    Fours = table.Column<int>(type: "int", nullable: false),
                    Sixes = table.Column<int>(type: "int", nullable: false),
                    StrikeRate = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattingFigures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattingFigures_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BattingFigures_Scorecards_ScorecardId",
                        column: x => x.ScorecardId,
                        principalTable: "Scorecards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BowlingFigures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScorecardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Overs = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Maidens = table.Column<int>(type: "int", nullable: false),
                    Runs = table.Column<int>(type: "int", nullable: false),
                    Wickets = table.Column<int>(type: "int", nullable: false),
                    NoBalls = table.Column<int>(type: "int", nullable: false),
                    Wides = table.Column<int>(type: "int", nullable: false),
                    Economy = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BowlingFigures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BowlingFigures_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BowlingFigures_Scorecards_ScorecardId",
                        column: x => x.ScorecardId,
                        principalTable: "Scorecards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattingFigures_PlayerId",
                table: "BattingFigures",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_BattingFigures_ScorecardId_PlayerId",
                table: "BattingFigures",
                columns: new[] { "ScorecardId", "PlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BowlingFigures_PlayerId",
                table: "BowlingFigures",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_BowlingFigures_ScorecardId_PlayerId",
                table: "BowlingFigures",
                columns: new[] { "ScorecardId", "PlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scorecards_BattingTeamId",
                table: "Scorecards",
                column: "BattingTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Scorecards_BowlingTeamId",
                table: "Scorecards",
                column: "BowlingTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Scorecards_FixtureId_InningsNo",
                table: "Scorecards",
                columns: new[] { "FixtureId", "InningsNo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattingFigures");

            migrationBuilder.DropTable(
                name: "BowlingFigures");

            migrationBuilder.DropTable(
                name: "Scorecards");
        }
    }
}
