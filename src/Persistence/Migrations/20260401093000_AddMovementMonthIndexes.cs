using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMovementMonthIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Statuses_MovementMonthId_Day",
                table: "Statuses",
                columns: new[] { "MovementMonthId", "Day" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_MonthMovements_MovementMonthId_Type_Paid",
                table: "MonthMovements",
                columns: new[] { "MovementMonthId", "Type", "Paid" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Statuses_MovementMonthId_Day",
                table: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_MonthMovements_MovementMonthId_Type_Paid",
                table: "MonthMovements");
        }
    }
}
