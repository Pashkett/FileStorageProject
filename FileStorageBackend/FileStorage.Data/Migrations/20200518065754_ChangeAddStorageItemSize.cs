using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorage.Data.Migrations
{
    public partial class ChangeAddStorageItemSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Size",
                table: "StorageItems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Size",
                table: "StorageItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
