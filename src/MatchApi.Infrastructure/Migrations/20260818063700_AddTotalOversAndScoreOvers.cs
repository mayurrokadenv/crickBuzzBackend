using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalOversAndScoreOvers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwayScore_Overs",
                table: "Fixtures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeScore_Overs",
                table: "Fixtures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TotalOvers",
                table: "Fixtures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayScore_Overs",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "HomeScore_Overs",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "TotalOvers",
                table: "Fixtures");
        }
    }
}
