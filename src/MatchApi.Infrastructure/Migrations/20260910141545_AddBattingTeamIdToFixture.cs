using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBattingTeamIdToFixture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BattingTeamId",
                table: "Fixtures",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BattingTeamId",
                table: "Fixtures");
        }
    }
}
