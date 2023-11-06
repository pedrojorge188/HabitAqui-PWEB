using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class RemoveUserTeste2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
            name: "UserId",
            table: "RentalContracts",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

            migrationBuilder.RenameColumn(
            name: "UserId",
            table: "RentalContracts",
            newName: "userId");
        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
