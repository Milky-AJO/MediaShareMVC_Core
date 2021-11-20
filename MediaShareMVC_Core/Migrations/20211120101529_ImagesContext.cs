using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaShareMVC_Core.Migrations
{
    public partial class ImagesContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MediaTitle = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    MediaName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MediaPublic = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.MediaId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Media");
        }
    }
}
