using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class UserTesteClass1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_habitacaos_locador_LocadorId",
                table: "habitacaos");

            migrationBuilder.DropForeignKey(
                name: "FK_locador_enrollments_enrollmentId",
                table: "locador");

            migrationBuilder.RenameColumn(
                name: "LocadorId",
                table: "habitacaos",
                newName: "locadorId");

            migrationBuilder.RenameIndex(
                name: "IX_habitacaos_LocadorId",
                table: "habitacaos",
                newName: "IX_habitacaos_locadorId");

            migrationBuilder.AlterColumn<int>(
                name: "enrollmentId",
                table: "locador",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LocatorId",
                table: "habitacaos",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_habitacaos_locador_locadorId",
                table: "habitacaos",
                column: "locadorId",
                principalTable: "locador",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_locador_enrollments_enrollmentId",
                table: "locador",
                column: "enrollmentId",
                principalTable: "enrollments",
                principalColumn: "Id");
        }
    }
}
