using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRecommenderAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedGameRecCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "GameRecommended",
                newName: "Title");

            migrationBuilder.AddColumn<int>(
                name: "Counter",
                table: "GameRecommended",
                type: "INT",
                maxLength: 8,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Counter",
                table: "GameRecommended");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "GameRecommended",
                newName: "Name");
        }
    }
}
