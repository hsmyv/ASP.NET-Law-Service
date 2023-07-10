using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspİntro.Migrations
{
    public partial class UpdatedSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomePostTake",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "LoadTake",
                table: "Settings");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Settings");

            migrationBuilder.AddColumn<int>(
                name: "HomePostTake",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoadTake",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
