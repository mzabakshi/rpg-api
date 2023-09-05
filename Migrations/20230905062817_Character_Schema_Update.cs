using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rpg_api.Migrations
{
    /// <inheritdoc />
    public partial class Character_Schema_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Intellingence",
                table: "Characters",
                newName: "Intelligence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Intelligence",
                table: "Characters",
                newName: "Intellingence");
        }
    }
}
