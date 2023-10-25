using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class Normalize7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "enrollmentStateId",
                table: "locador",
                newName: "enrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_locador_enrollments_enrollmentId",
                table: "locador",
                column: "enrollmentId",
                principalTable: "enrollments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
