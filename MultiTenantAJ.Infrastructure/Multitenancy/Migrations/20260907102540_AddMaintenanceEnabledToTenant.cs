using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenantAJ.Infrastructure.Multitenancy.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceEnabledToTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MaintenanceEnabled",
                schema: "MultiTenancy",
                table: "Tenants",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaintenanceEnabled",
                schema: "MultiTenancy",
                table: "Tenants");
        }
    }
}
