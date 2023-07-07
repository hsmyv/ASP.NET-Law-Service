using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspİntro.Migrations
{
    public partial class UpdatedSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoadTake",
                table: "Settings",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "HomePostTake",
                table: "Settings",
                newName: "Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Settings",
                newName: "LoadTake");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Settings",
                newName: "HomePostTake");
        }
    }
}
