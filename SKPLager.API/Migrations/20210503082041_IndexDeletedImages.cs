using Microsoft.EntityFrameworkCore.Migrations;

namespace SKPLager.API.Migrations
{
    public partial class IndexDeletedImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Images_IsDeleted",
                table: "Images",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_IsDeleted",
                table: "Images");
        }
    }
}
