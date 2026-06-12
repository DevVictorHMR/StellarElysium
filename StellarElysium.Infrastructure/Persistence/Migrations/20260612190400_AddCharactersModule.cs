using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarElysium.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCharactersModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    GameCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rarity = table.Column<int>(type: "int", nullable: false),
                    Element = table.Column<int>(type: "int", nullable: false),
                    WeaponType = table.Column<int>(type: "int", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Association = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BirthdayDay = table.Column<int>(type: "int", nullable: true),
                    BirthdayMonth = table.Column<int>(type: "int", nullable: true),
                    Constellation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChineseVoiceActor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    JapaneseVoiceActor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EnglishVoiceActor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    KoreanVoiceActor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProfileLocalizationsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterAscensionMaterialsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillAscensionMaterialsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillAscensionJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedItemsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoundsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuotesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoriesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Slug",
                table: "Characters",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
