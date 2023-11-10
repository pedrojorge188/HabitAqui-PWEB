using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class updateLocador : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_managers_LocadorId",
                table: "managers");

            migrationBuilder.CreateIndex(
                name: "IX_managers_LocadorId",
                table: "managers",
                column: "LocadorId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_managers_LocadorId",
                table: "managers");

            migrationBuilder.CreateIndex(
                name: "IX_managers_LocadorId",
                table: "managers",
                column: "LocadorId");
        }
    }
}
