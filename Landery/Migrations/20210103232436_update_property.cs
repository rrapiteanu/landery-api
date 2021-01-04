using Microsoft.EntityFrameworkCore.Migrations;

namespace Landery.Migrations
{
    public partial class update_property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Properties");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Properties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Properties",
                type: "text",
                nullable: true);
        }
    }
}
