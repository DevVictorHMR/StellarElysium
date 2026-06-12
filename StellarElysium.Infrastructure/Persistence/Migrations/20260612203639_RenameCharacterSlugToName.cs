using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarElysium.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameCharacterSlugToName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Characters",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_Slug",
                table: "Characters",
                newName: "IX_Characters_Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Characters",
                newName: "Slug");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_Name",
                table: "Characters",
                newName: "IX_Characters_Slug");
        }
    }
}
