using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class Normalize3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_locador_enrollments_enrollmentStateId",
                table: "locador");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_locador_enrollments_enrollmentId",
                table: "locador");

            migrationBuilder.RenameColumn(
                name: "enrollmentId",
                table: "locador",
                newName: "EnrollmentId");

            migrationBuilder.RenameIndex(
                name: "IX_locador_enrollmentId",
                table: "locador",
                newName: "IX_locador_EnrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_locador_enrollments_EnrollmentId",
                table: "locador",
                column: "EnrollmentId",
                principalTable: "enrollments",
                principalColumn: "Id");
        }
    }
}
