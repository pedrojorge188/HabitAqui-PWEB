using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui_Software.Data.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "deliveryStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveryStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "enrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "receiveStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receiveStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "locador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    enrollmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_locador_enrollments_enrollmentId",
                        column: x => x.enrollmentId,
                        principalTable: "enrollments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "habitacaos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: true),
                    rentalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    startDateAvailability = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDateAvailability = table.Column<DateTime>(type: "datetime2", nullable: false),
                    minimumRentalPeriod = table.Column<int>(type: "int", nullable: false),
                    maximumRentalPeriod = table.Column<int>(type: "int", nullable: false),
                    available = table.Column<bool>(type: "bit", nullable: false),
                    grade = table.Column<int>(type: "int", nullable: false),
                    locadorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_habitacaos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_habitacaos_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_habitacaos_locador_locadorId",
                        column: x => x.locadorId,
                        principalTable: "locador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rentalContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    habitacaoId = table.Column<int>(type: "int", nullable: true),
                    receiveStatusId = table.Column<int>(type: "int", nullable: true),
                    deliveryStatusId = table.Column<int>(type: "int", nullable: true),
                    isConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentalContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rentalContracts_deliveryStatus_receiveStatusId",
                        column: x => x.receiveStatusId,
                        principalTable: "deliveryStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_rentalContracts_habitacaos_habitacaoId",
                        column: x => x.habitacaoId,
                        principalTable: "habitacaos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_rentalContracts_receiveStatus_deliveryStatusId",
                        column: x => x.deliveryStatusId,
                        principalTable: "receiveStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_habitacaos_categoryId",
                table: "habitacaos",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_habitacaos_locadorId",
                table: "habitacaos",
                column: "locadorId");

            migrationBuilder.CreateIndex(
                name: "IX_locador_enrollmentId",
                table: "locador",
                column: "enrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_rentalContracts_deliveryStatusId",
                table: "rentalContracts",
                column: "deliveryStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_rentalContracts_habitacaoId",
                table: "rentalContracts",
                column: "habitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_rentalContracts_receiveStatusId",
                table: "rentalContracts",
                column: "receiveStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentalContracts");

            migrationBuilder.DropTable(
                name: "deliveryStatus");

            migrationBuilder.DropTable(
                name: "habitacaos");

            migrationBuilder.DropTable(
                name: "receiveStatus");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "locador");

            migrationBuilder.DropTable(
                name: "enrollments");
        }
    }
}
