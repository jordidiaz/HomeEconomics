using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
#pragma warning disable CS8981
    public partial class paid : Migration
#pragma warning restore CS8981
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "MonthMovements",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "MonthMovements");
        }
    }
}
