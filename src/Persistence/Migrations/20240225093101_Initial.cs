using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovementMonths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementMonths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthMovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Paid = table.Column<bool>(type: "boolean", nullable: false),
                    MovementMonthId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthMovements_MovementMonths_MovementMonthId",
                        column: x => x.MovementMonthId,
                        principalTable: "MovementMonths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    AccountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CashAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MovementMonthId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statuses_MovementMonths_MovementMonthId",
                        column: x => x.MovementMonthId,
                        principalTable: "MovementMonths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Frequencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Months = table.Column<string>(type: "character varying(23)", maxLength: 23, nullable: false),
                    MovementId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frequencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Frequencies_Movements_MovementId",
                        column: x => x.MovementId,
                        principalTable: "Movements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Frequencies_MovementId",
                table: "Frequencies",
                column: "MovementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthMovements_MovementMonthId",
                table: "MonthMovements",
                column: "MovementMonthId");

            migrationBuilder.CreateIndex(
                name: "IX_MovementMonths_Year_Month",
                table: "MovementMonths",
                columns: new[] { "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movements_Name",
                table: "Movements",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_MovementMonthId",
                table: "Statuses",
                column: "MovementMonthId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Frequencies");

            migrationBuilder.DropTable(
                name: "MonthMovements");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "MovementMonths");
        }
    }
}
