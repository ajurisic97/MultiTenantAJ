using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenantAJ.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeRoleTenantAware : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                schema: "Identity",
                table: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Identity",
                table: "Roles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId_Name",
                schema: "Identity",
                table: "Roles",
                columns: new[] { "TenantId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_TenantId_Name",
                schema: "Identity",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Identity",
                table: "Roles");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "Identity",
                table: "Roles",
                column: "Name",
                unique: true);
        }
    }
}
