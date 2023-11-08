using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class MissingProperties1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasEquipments",
                table: "DeliveryStatus",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasDamage",
                table: "DeliveryStatus",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "DeliveryStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasEquipments",
                table: "ReceiveStatus",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasDamage",
                table: "ReceiveStatus",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "ReceiveStatus",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasEquipments",
                table: "DeliveryStatus");

            migrationBuilder.DropColumn(
                name: "HasDamage",
                table: "DeliveryStatus");

            migrationBuilder.DropColumn(
                name: "Observation",
                table: "DeliveryStatus");

            migrationBuilder.DropColumn(
                name: "HasEquipments",
                table: "ReceiveStatus");

            migrationBuilder.DropColumn(
                name: "HasDamage",
                table: "ReceiveStatus");

            migrationBuilder.DropColumn(
                name: "Observation",
                table: "ReceiveStatus");
        }
    }
}
