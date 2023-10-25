using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class Normalize5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rentalContracts_AspNetUsers_userId",
                table: "rentalContracts");

            

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rentalContracts_AspNetUsers_userId",
                table: "rentalContracts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "rentalContracts");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "rentalContracts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_rentalContracts_userId",
                table: "rentalContracts",
                newName: "IX_rentalContracts_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_rentalContracts_AspNetUsers_UserId",
                table: "rentalContracts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
