using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenantAJ.Infrastructure.Multitenancy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTenantIdentification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenants_Identifier",
                schema: "MultiTenancy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Identifier",
                schema: "MultiTenancy",
                table: "Tenants");

            migrationBuilder.AddColumn<Guid>(
                name: "ApiKey",
                schema: "MultiTenancy",
                table: "Tenants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_ApiKey",
                schema: "MultiTenancy",
                table: "Tenants",
                column: "ApiKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenants_ApiKey",
                schema: "MultiTenancy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                schema: "MultiTenancy",
                table: "Tenants");

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                schema: "MultiTenancy",
                table: "Tenants",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Identifier",
                schema: "MultiTenancy",
                table: "Tenants",
                column: "Identifier",
                unique: true);
        }
    }
}
