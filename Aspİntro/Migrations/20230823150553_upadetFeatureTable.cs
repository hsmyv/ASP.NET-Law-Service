using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspİntro.Migrations
{
    public partial class upadetFeatureTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "label1Image",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "label2Image",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "label3Image",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "label4Image",
                table: "Features");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "label1Image",
                table: "Features",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "label2Image",
                table: "Features",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "label3Image",
                table: "Features",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "label4Image",
                table: "Features",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
