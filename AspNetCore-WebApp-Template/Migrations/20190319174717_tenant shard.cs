using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabasePerTenantPOC.Migrations
{
    public partial class tenantshard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantShardKey",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantShardKey",
                table: "AspNetUsers");
        }
    }
}
