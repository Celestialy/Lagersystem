using Microsoft.EntityFrameworkCore.Migrations;

namespace SKPLager.API.Migrations
{
    public partial class renameBarcodeOnItemEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Barocde",
                table: "Items",
                newName: "Barcode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Barcode",
                table: "Items",
                newName: "Barocde");
        }
    }
}
