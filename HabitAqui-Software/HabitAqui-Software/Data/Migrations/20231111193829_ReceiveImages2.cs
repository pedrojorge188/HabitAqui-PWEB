using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class ReceiveImages2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "receiveStatus",
                newName: "ImagePaths");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePaths",
                table: "receiveStatus",
                newName: "ImagePath");
        }
    }
}
