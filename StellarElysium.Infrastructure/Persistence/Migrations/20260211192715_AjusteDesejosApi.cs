using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarElysium.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AjusteDesejosApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Desejos_ContaGenshinId_TipoBanner_DataHoraDesejo",
                table: "Desejos");

            migrationBuilder.RenameColumn(
                name: "TipoBanner",
                table: "Desejos",
                newName: "RankType");

            migrationBuilder.RenameColumn(
                name: "Rareza",
                table: "Desejos",
                newName: "Quantidade");

            migrationBuilder.RenameColumn(
                name: "IdDesejoExterno",
                table: "Desejos",
                newName: "PullId");

            migrationBuilder.RenameIndex(
                name: "IX_Desejos_ContaGenshinId_IdDesejoExterno",
                table: "Desejos",
                newName: "IX_Desejos_ContaGenshinId_PullId");

            migrationBuilder.AlterColumn<string>(
                name: "TipoItem",
                table: "Desejos",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GachaType",
                table: "Desejos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                table: "Desejos",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Desejos_ContaGenshinId_GachaType_DataHoraDesejo",
                table: "Desejos",
                columns: new[] { "ContaGenshinId", "GachaType", "DataHoraDesejo" });

            migrationBuilder.CreateIndex(
                name: "IX_ContasGenshin_Uid_Servidor",
                table: "ContasGenshin",
                columns: new[] { "Uid", "Servidor" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Desejos_ContaGenshinId_GachaType_DataHoraDesejo",
                table: "Desejos");

            migrationBuilder.DropIndex(
                name: "IX_ContasGenshin_Uid_Servidor",
                table: "ContasGenshin");

            migrationBuilder.DropColumn(
                name: "GachaType",
                table: "Desejos");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Desejos");

            migrationBuilder.RenameColumn(
                name: "RankType",
                table: "Desejos",
                newName: "TipoBanner");

            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "Desejos",
                newName: "Rareza");

            migrationBuilder.RenameColumn(
                name: "PullId",
                table: "Desejos",
                newName: "IdDesejoExterno");

            migrationBuilder.RenameIndex(
                name: "IX_Desejos_ContaGenshinId_PullId",
                table: "Desejos",
                newName: "IX_Desejos_ContaGenshinId_IdDesejoExterno");

            migrationBuilder.AlterColumn<int>(
                name: "TipoItem",
                table: "Desejos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.CreateIndex(
                name: "IX_Desejos_ContaGenshinId_TipoBanner_DataHoraDesejo",
                table: "Desejos",
                columns: new[] { "ContaGenshinId", "TipoBanner", "DataHoraDesejo" });
        }
    }
}
