using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorage.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 250, nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    UserRootFolderId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrustedName = table.Column<string>(maxLength: 300, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 300, nullable: false),
                    IsFolder = table.Column<bool>(nullable: false),
                    IsRootFolder = table.Column<bool>(nullable: true),
                    RelativePath = table.Column<string>(maxLength: 900, nullable: false),
                    ParentFolderId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorageItems_StorageItems_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "StorageItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StorageItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_ParentFolderId",
                table: "StorageItems",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_UserId",
                table: "StorageItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRootFolderId",
                table: "Users",
                column: "UserRootFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_StorageItems_UserRootFolderId",
                table: "Users",
                column: "UserRootFolderId",
                principalTable: "StorageItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItems_Users_UserId",
                table: "StorageItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "StorageItems");
        }
    }
}
