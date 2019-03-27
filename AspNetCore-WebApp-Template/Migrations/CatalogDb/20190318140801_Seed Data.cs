using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabasePerTenantPOC.Migrations.CatalogDb
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Databases",
                columns: new[] { "ServerName", "DatabaseName", "ElasticPoolName", "LastUpdated", "RecoveryState", "ServiceObjective", "State" },
                values: new object[] { "localdb", "MSSQLLocalDb", "local", new DateTime(2019, 3, 18, 10, 8, 1, 2, DateTimeKind.Local).AddTicks(9231), "", "", "" });

            migrationBuilder.InsertData(
                table: "ElasticPools",
                columns: new[] { "ServerName", "ElasticPoolName", "DatabaseDtuMax", "DatabaseDtuMin", "Dtu", "Edition", "LastUpdated", "RecoveryState", "State", "StorageMB" },
                values: new object[] { "localdb", "local", 0, 0, 0, "", new DateTime(2019, 3, 18, 10, 8, 1, 2, DateTimeKind.Local).AddTicks(5641), "", "", 0 });

            migrationBuilder.InsertData(
                table: "Servers",
                columns: new[] { "ServerName", "LastUpdated", "Location", "RecoveryState", "State" },
                values: new object[] { "localdb", new DateTime(2019, 3, 18, 10, 8, 0, 997, DateTimeKind.Local).AddTicks(3918), "", "", "" });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "TenantId", "LastUpdated", "RecoveryState", "ServicePlan", "TenantName" },
                values: new object[] { new byte[] { 1 }, new DateTime(2019, 3, 18, 10, 8, 1, 3, DateTimeKind.Local).AddTicks(1891), "", "", "localTenant" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Databases",
                keyColumns: new[] { "ServerName", "DatabaseName" },
                keyValues: new object[] { "localdb", "MSSQLLocalDb" });

            migrationBuilder.DeleteData(
                table: "ElasticPools",
                keyColumns: new[] { "ServerName", "ElasticPoolName" },
                keyValues: new object[] { "localdb", "local" });

            migrationBuilder.DeleteData(
                table: "Servers",
                keyColumn: "ServerName",
                keyValue: "localdb");

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: new byte[] { 1 });
        }
    }
}
