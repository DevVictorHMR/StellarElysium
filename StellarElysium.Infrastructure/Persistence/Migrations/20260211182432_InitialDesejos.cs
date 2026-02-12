using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarElysium.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialDesejos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContasGenshin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Uid = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Apelido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Servidor = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasGenshin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Desejos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContaGenshinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDesejoExterno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoBanner = table.Column<int>(type: "int", nullable: false),
                    TipoItem = table.Column<int>(type: "int", nullable: false),
                    NomeItem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rareza = table.Column<int>(type: "int", nullable: false),
                    DataHoraDesejo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desejos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Desejos_ContasGenshin_ContaGenshinId",
                        column: x => x.ContaGenshinId,
                        principalTable: "ContasGenshin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Desejos_ContaGenshinId_IdDesejoExterno",
                table: "Desejos",
                columns: new[] { "ContaGenshinId", "IdDesejoExterno" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Desejos_ContaGenshinId_TipoBanner_DataHoraDesejo",
                table: "Desejos",
                columns: new[] { "ContaGenshinId", "TipoBanner", "DataHoraDesejo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Desejos");

            migrationBuilder.DropTable(
                name: "ContasGenshin");
        }
    }
}
