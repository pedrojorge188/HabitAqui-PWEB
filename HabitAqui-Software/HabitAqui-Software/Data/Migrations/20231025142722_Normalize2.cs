using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class Normalize2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_locador_enrollments_enrollmentStateId",
                table: "locador");

            migrationBuilder.RenameColumn(
                name: "enrollmentStateId",
                table: "locador",
                newName: "enrollmentStateId");

            migrationBuilder.RenameIndex(
                name: "IX_locador_enrollmentStateId",
                table: "locador",
                newName: "IX_locador_enrollmentStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_locador_enrollments_enrollmentStateId",
                table: "locador",
                column: "enrollmentStateId",
                principalTable: "enrollments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_locador_enrollments_EnrollmentId",
                table: "locador");

            migrationBuilder.RenameColumn(
                name: "EnrollmentId",
                table: "locador",
                newName: "enrollmentId");

            migrationBuilder.RenameIndex(
                name: "IX_locador_EnrollmentId",
                table: "locador",
                newName: "IX_locador_enrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_locador_enrollments_enrollmentId",
                table: "locador",
                column: "enrollmentId",
                principalTable: "enrollments",
                principalColumn: "Id");
        }
    }
}
