using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorage.Data.Migrations
{
    public partial class AddStorageItemSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "StorageItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "StorageItems");
        }
    }
}
