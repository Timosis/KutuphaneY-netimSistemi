using Microsoft.EntityFrameworkCore.Migrations;

namespace Kys.Web.Migrations
{
    public partial class keq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KitapOduncDurum",
                table: "KitapOdunc",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KitapOduncDurum",
                table: "KitapOdunc");
        }
    }
}
