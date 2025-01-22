using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HsR.Journal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceRelevance",
                table: "Entries",
                newName: "UnitPriceRelevance");

            migrationBuilder.RenameColumn(
                name: "CostRelevance",
                table: "Entries",
                newName: "TotalCostRelevance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPriceRelevance",
                table: "Entries",
                newName: "PriceRelevance");

            migrationBuilder.RenameColumn(
                name: "TotalCostRelevance",
                table: "Entries",
                newName: "CostRelevance");
        }
    }
}
