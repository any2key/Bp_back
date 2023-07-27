using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bp_back.Migrations
{
    public partial class hubport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "Hubs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "Hubs");
        }
    }
}
