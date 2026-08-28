using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenantAJ.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "Catalog");
            migrationBuilder.Sql("DROP SCHEMA IF EXISTS \"Catalog\";");
            migrationBuilder.Sql("DROP SCHEMA IF EXISTS \"Data\";");

            migrationBuilder.EnsureSchema(
                name: "PropertyManagement");

            migrationBuilder.CreateTable(
                name: "Guests",
                schema: "PropertyManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                schema: "PropertyManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    NumberOfRooms = table.Column<int>(type: "integer", nullable: false),
                    NumberOfBeds = table.Column<int>(type: "integer", nullable: false),
                    MaximumCapacity = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRequests",
                schema: "PropertyManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "PropertyManagement",
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                schema: "PropertyManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    GuestId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Guests_GuestId",
                        column: x => x.GuestId,
                        principalSchema: "PropertyManagement",
                        principalTable: "Guests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "PropertyManagement",
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guests_TenantId",
                schema: "PropertyManagement",
                table: "Guests",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_PropertyId",
                schema: "PropertyManagement",
                table: "MaintenanceRequests",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_TenantId",
                schema: "PropertyManagement",
                table: "MaintenanceRequests",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_TenantId",
                schema: "PropertyManagement",
                table: "Properties",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_GuestId",
                schema: "PropertyManagement",
                table: "Reservations",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PropertyId",
                schema: "PropertyManagement",
                table: "Reservations",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TenantId",
                schema: "PropertyManagement",
                table: "Reservations",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceRequests",
                schema: "PropertyManagement");

            migrationBuilder.DropTable(
                name: "Reservations",
                schema: "PropertyManagement");

            migrationBuilder.DropTable(
                name: "Guests",
                schema: "PropertyManagement");

            migrationBuilder.DropTable(
                name: "Properties",
                schema: "PropertyManagement");

            migrationBuilder.EnsureSchema(
                name: "Catalog");

            migrationBuilder.EnsureSchema(
                name: "Data");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_TenantId",
                schema: "Catalog",
                table: "Products",
                column: "TenantId");
        }
    }
}
