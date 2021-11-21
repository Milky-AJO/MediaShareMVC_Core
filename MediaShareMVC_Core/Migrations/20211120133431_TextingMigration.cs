using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaShareMVC_Core.Migrations
{
    public partial class TextingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Media");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MediaPublic = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    MediaTitle = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.MediaId);
                });
        }
    }
}
