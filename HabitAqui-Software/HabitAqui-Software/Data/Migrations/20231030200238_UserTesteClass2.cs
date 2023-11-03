using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class UserTesteClass2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTesteId",
                table: "rentalContracts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "userTeste",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    locadorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userTeste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userTeste_locador_locadorId",
                        column: x => x.locadorId,
                        principalTable: "locador",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_rentalContracts_UserTesteId",
                table: "rentalContracts",
                column: "UserTesteId");

            migrationBuilder.CreateIndex(
                name: "IX_userTeste_locadorId",
                table: "userTeste",
                column: "locadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_rentalContracts_userTeste_UserTesteId",
                table: "rentalContracts",
                column: "UserTesteId",
                principalTable: "userTeste",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rentalContracts_userTeste_UserTesteId",
                table: "rentalContracts");

            migrationBuilder.DropTable(
                name: "userTeste");

            migrationBuilder.DropIndex(
                name: "IX_rentalContracts_UserTesteId",
                table: "rentalContracts");

            migrationBuilder.DropColumn(
                name: "UserTesteId",
                table: "rentalContracts");
        }
    }
}
