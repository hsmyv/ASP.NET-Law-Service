using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspİntro.Migrations
{
    public partial class AddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "XTake",
                table: "Settings",
                newName: "HomePostTake");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HomePostTake",
                table: "Settings",
                newName: "XTake");
        }
    }
}
