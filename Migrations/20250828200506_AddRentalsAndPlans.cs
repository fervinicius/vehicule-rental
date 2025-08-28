using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace rental_challenge.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalsAndPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "rentals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "rentals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PredictedEndDate",
                table: "rentals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "RentalPlanId",
                table: "rentals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "rentals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "rentalPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DurationInDays = table.Column<int>(type: "integer", nullable: false),
                    DailyCost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentalPlans", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "rentalPlans",
                columns: new[] { "Id", "DailyCost", "DurationInDays" },
                values: new object[,]
                {
                    { new Guid("1c3e4567-e89b-12d3-a456-426614174000"), 20.00m, 45 },
                    { new Guid("2d4f5678-e89b-12d3-a456-426614174001"), 18.00m, 50 },
                    { new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"), 22.00m, 30 },
                    { new Guid("b1a3c7c2-8e4d-4e9f-8d9e-3e2b1a0f8c7d"), 28.00m, 15 },
                    { new Guid("f81b1a72-91f3-4e67-a72a-c2a4f0b2f3a6"), 30.00m, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentalPlans");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "rentals");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "rentals");

            migrationBuilder.DropColumn(
                name: "PredictedEndDate",
                table: "rentals");

            migrationBuilder.DropColumn(
                name: "RentalPlanId",
                table: "rentals");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "rentals");
        }
    }
}
