using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class Normalize1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_habitacaos_locador_locadorId",
                table: "habitacaos");

            

            migrationBuilder.DropForeignKey(
                name: "FK_rentalContracts_AspNetUsers_userId",
                table: "rentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_rentalContracts_deliveryStatus_receiveStatusId",
                table: "rentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_rentalContracts_habitacaos_habitacaoId",
                table: "rentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_rentalContracts_receiveStatus_deliveryStatusId",
                table: "rentalContracts");

            migrationBuilder.DropIndex(
                name: "IX_rentalContracts_deliveryStatusId",
                table: "rentalContracts");

            migrationBuilder.DropIndex(
                name: "IX_rentalContracts_receiveStatusId",
                table: "rentalContracts");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "rentalContracts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "receiveStatusId",
                table: "rentalContracts",
                newName: "ReceiveStatusId");

            migrationBuilder.RenameColumn(
                name: "habitacaoId",
                table: "rentalContracts",
                newName: "HabitacaoId");

            migrationBuilder.RenameColumn(
                name: "deliveryStatusId",
                table: "rentalContracts",
                newName: "DeliveryStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_rentalContracts_userId",
                table: "rentalContracts",
                newName: "IX_rentalContracts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_rentalContracts_habitacaoId",
                table: "rentalContracts",
                newName: "IX_rentalContracts_HabitacaoId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "rentalContracts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "rentalContractId",
                table: "receiveStatus",
                type: "int",
                nullable: true);

            

            migrationBuilder.AlterColumn<int>(
                name: "locadorId",
                table: "habitacaos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LocatorId",
                table: "habitacaos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentalContractId",
                table: "deliveryStatus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_receiveStatus_rentalContractId",
                table: "receiveStatus",
                column: "rentalContractId",
                unique: true,
                filter: "[rentalContractId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_deliveryStatus_RentalContractId",
                table: "deliveryStatus",
                column: "RentalContractId",
                unique: true,
                filter: "[RentalContractId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_deliveryStatus_rentalContracts_RentalContractId",
                table: "deliveryStatus",
                column: "RentalContractId",
                principalTable: "rentalContracts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_habitacaos_locador_locadorId",
                table: "habitacaos",
                column: "locadorId",
                principalTable: "locador",
                principalColumn: "Id");

            

            migrationBuilder.AddForeignKey(
                name: "FK_receiveStatus_rentalContracts_rentalContractId",
                table: "receiveStatus",
                column: "rentalContractId",
                principalTable: "rentalContracts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_rentalContracts_AspNetUsers_UserId",
                table: "rentalContracts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_rentalContracts_habitacaos_HabitacaoId",
                table: "rentalContracts",
                column: "HabitacaoId",
                principalTable: "habitacaos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deliveryStatus_rentalContracts_RentalContractId",
                table: "deliveryStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_habitacaos_locador_locadorId",
                table: "habitacaos");

            migrationBuilder.DropForeignKey(
                name: "FK_locador_enrollments_enrollmentId",
                table: "locador");

            migrationBuilder.DropForeignKey(
                name: "FK_receiveStatus_rentalContracts_rentalContractId",
                table: "receiveStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_rentalContracts_AspNetUsers_UserId",
                table: "rentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_rentalContracts_habitacaos_HabitacaoId",
                table: "rentalContracts");

            migrationBuilder.DropIndex(
                name: "IX_receiveStatus_rentalContractId",
                table: "receiveStatus");

            migrationBuilder.DropIndex(
                name: "IX_deliveryStatus_RentalContractId",
                table: "deliveryStatus");

            migrationBuilder.DropColumn(
                name: "rentalContractId",
                table: "receiveStatus");

            migrationBuilder.DropColumn(
                name: "LocatorId",
                table: "habitacaos");

            migrationBuilder.DropColumn(
                name: "RentalContractId",
                table: "deliveryStatus");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "rentalContracts",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "ReceiveStatusId",
                table: "rentalContracts",
                newName: "receiveStatusId");

            migrationBuilder.RenameColumn(
                name: "HabitacaoId",
                table: "rentalContracts",
                newName: "habitacaoId");

            migrationBuilder.RenameColumn(
                name: "DeliveryStatusId",
                table: "rentalContracts",
                newName: "deliveryStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_rentalContracts_UserId",
                table: "rentalContracts",
                newName: "IX_rentalContracts_userId");

            migrationBuilder.RenameIndex(
                name: "IX_rentalContracts_HabitacaoId",
                table: "rentalContracts",
                newName: "IX_rentalContracts_habitacaoId");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "rentalContracts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "enrollmentId",
                table: "locador",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "locadorId",
                table: "habitacaos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_rentalContracts_deliveryStatusId",
                table: "rentalContracts",
                column: "deliveryStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_rentalContracts_receiveStatusId",
                table: "rentalContracts",
                column: "receiveStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_habitacaos_locador_locadorId",
                table: "habitacaos",
                column: "locadorId",
                principalTable: "locador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_locador_enrollments_enrollmentId",
                table: "locador",
                column: "enrollmentId",
                principalTable: "enrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rentalContracts_AspNetUsers_userId",
                table: "rentalContracts",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rentalContracts_deliveryStatus_receiveStatusId",
                table: "rentalContracts",
                column: "receiveStatusId",
                principalTable: "deliveryStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_rentalContracts_habitacaos_habitacaoId",
                table: "rentalContracts",
                column: "habitacaoId",
                principalTable: "habitacaos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_rentalContracts_receiveStatus_deliveryStatusId",
                table: "rentalContracts",
                column: "deliveryStatusId",
                principalTable: "receiveStatus",
                principalColumn: "Id");
        }
    }
}
