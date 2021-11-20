using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaShareMVC_Core.Migrations
{
    public partial class ImageContextWithUsersFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Media",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Media");
        }
    }
}
