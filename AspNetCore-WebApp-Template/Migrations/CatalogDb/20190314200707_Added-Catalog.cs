using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabasePerTenantPOC.Migrations.CatalogDb
{
    public partial class AddedCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Databases",
                columns: table => new
                {
                    ServerName = table.Column<string>(maxLength: 128, nullable: false),
                    DatabaseName = table.Column<string>(maxLength: 128, nullable: false),
                    ServiceObjective = table.Column<string>(maxLength: 50, nullable: false),
                    ElasticPoolName = table.Column<string>(maxLength: 128, nullable: false),
                    State = table.Column<string>(maxLength: 30, nullable: false),
                    RecoveryState = table.Column<string>(maxLength: 30, nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Databases", x => new { x.ServerName, x.DatabaseName });
                });

            migrationBuilder.CreateTable(
                name: "ElasticPools",
                columns: table => new
                {
                    ServerName = table.Column<string>(nullable: false),
                    ElasticPoolName = table.Column<string>(nullable: false),
                    Edition = table.Column<string>(maxLength: 20, nullable: false),
                    Dtu = table.Column<int>(nullable: false),
                    DatabaseDtuMax = table.Column<int>(nullable: false),
                    DatabaseDtuMin = table.Column<int>(nullable: false),
                    StorageMB = table.Column<int>(nullable: false),
                    State = table.Column<string>(maxLength: 30, nullable: false),
                    RecoveryState = table.Column<string>(maxLength: 30, nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElasticPools", x => new { x.ServerName, x.ElasticPoolName });
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    ServerName = table.Column<string>(nullable: false),
                    Location = table.Column<string>(maxLength: 30, nullable: false),
                    State = table.Column<string>(maxLength: 30, nullable: false),
                    RecoveryState = table.Column<string>(maxLength: 30, nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.ServerName);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<byte[]>(maxLength: 128, nullable: false),
                    TenantName = table.Column<string>(maxLength: 50, nullable: false),
                    ServicePlan = table.Column<string>(maxLength: 30, nullable: false, defaultValueSql: "'standard'"),
                    RecoveryState = table.Column<string>(maxLength: 30, nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tenants__2E9B47E15565CFCB", x => x.TenantId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_TenantName",
                table: "Tenants",
                column: "TenantName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Databases");

            migrationBuilder.DropTable(
                name: "ElasticPools");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
