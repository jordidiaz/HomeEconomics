using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Statuses_MovementMonthId",
                table: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_MonthMovements_MovementMonthId",
                table: "MonthMovements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Statuses_MovementMonthId",
                table: "Statuses",
                column: "MovementMonthId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthMovements_MovementMonthId",
                table: "MonthMovements",
                column: "MovementMonthId");
        }
    }
}
