using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class userchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "confirmed",
                table: "AspNetUsers",
                newName: "available");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
