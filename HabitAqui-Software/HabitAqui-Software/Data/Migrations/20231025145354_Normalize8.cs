using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class Normalize8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "locadorId",
                table: "habitacaos",
                newName: "LocadorId");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
