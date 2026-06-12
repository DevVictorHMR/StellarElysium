using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarElysium.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialWishes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenshinAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Uid = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Server = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenshinAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenshinAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PullId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GachaType = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RankType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    WishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wishes_GenshinAccounts_GenshinAccountId",
                        column: x => x.GenshinAccountId,
                        principalTable: "GenshinAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenshinAccounts_Uid_Server",
                table: "GenshinAccounts",
                columns: new[] { "Uid", "Server" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishes_GenshinAccountId_GachaType_WishTime",
                table: "Wishes",
                columns: new[] { "GenshinAccountId", "GachaType", "WishTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Wishes_GenshinAccountId_PullId",
                table: "Wishes",
                columns: new[] { "GenshinAccountId", "PullId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wishes");

            migrationBuilder.DropTable(
                name: "GenshinAccounts");
        }
    }
}
