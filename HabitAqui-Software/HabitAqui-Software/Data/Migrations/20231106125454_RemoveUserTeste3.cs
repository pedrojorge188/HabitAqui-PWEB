using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class RemoveUserTeste3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
            name: "UserId",
            table: "RentalContracts",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "nvarchar(max)",
            oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
