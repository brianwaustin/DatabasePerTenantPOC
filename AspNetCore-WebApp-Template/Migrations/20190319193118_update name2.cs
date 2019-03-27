using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabasePerTenantPOC.Migrations
{
    public partial class updatename2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenantShardKey",
                table: "AspNetUsers",
                newName: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "AspNetUsers",
                newName: "TenantShardKey");
        }
    }
}
