using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorage.Data.Migrations
{
    public partial class ExtendStorageItemRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "StorageItems",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecycled",
                table: "StorageItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "StorageItems");

            migrationBuilder.DropColumn(
                name: "IsRecycled",
                table: "StorageItems");
        }
    }
}
