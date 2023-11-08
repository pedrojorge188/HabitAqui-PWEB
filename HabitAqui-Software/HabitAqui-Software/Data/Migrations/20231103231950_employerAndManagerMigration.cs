using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class employerAndManagerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_locador_locadorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_locadorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "locadorId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "employers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocadorId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_employers_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_employers_locador_LocadorId",
                        column: x => x.LocadorId,
                        principalTable: "locador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "managers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocadorId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_managers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_managers_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_managers_locador_LocadorId",
                        column: x => x.LocadorId,
                        principalTable: "locador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employers_LocadorId",
                table: "employers",
                column: "LocadorId");

            migrationBuilder.CreateIndex(
                name: "IX_employers_userId",
                table: "employers",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_managers_LocadorId",
                table: "managers",
                column: "LocadorId");

            migrationBuilder.CreateIndex(
                name: "IX_managers_userId",
                table: "managers",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        
        }
    }
}
