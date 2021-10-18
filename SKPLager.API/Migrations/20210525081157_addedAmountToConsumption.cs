using Microsoft.EntityFrameworkCore.Migrations;

namespace SKPLager.API.Migrations
{
    public partial class addedAmountToConsumption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "ConsumptionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ConsumptionItems");
        }
    }
}
