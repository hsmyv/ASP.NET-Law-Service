using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspİntro.Migrations
{
    public partial class featureToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    label1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    label1Count = table.Column<int>(type: "int", nullable: false),
                    label1Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    label2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    label2Count = table.Column<int>(type: "int", nullable: false),
                    label2Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    label3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    label3Count = table.Column<int>(type: "int", nullable: false),
                    label3Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    label4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    label4Count = table.Column<int>(type: "int", nullable: false),
                    label4Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

          
        }
    }
}
