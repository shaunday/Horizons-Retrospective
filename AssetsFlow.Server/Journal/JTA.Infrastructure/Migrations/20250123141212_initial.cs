using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HsR.Journal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeElements_TradeComposites_CompositeFK",
                table: "TradeElements");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeElements_TradeComposites_CompositeRefId",
                table: "TradeElements");

            migrationBuilder.DropIndex(
                name: "IX_TradeElements_CompositeFK",
                table: "TradeElements");

            migrationBuilder.DropIndex(
                name: "IX_TradeElements_CompositeRefId",
                table: "TradeElements");

            migrationBuilder.DropColumn(
                name: "CompositeRefId",
                table: "TradeElements");

            migrationBuilder.AddColumn<int>(
                name: "SummaryId",
                table: "TradeComposites",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeElements_CompositeFK",
                table: "TradeElements",
                column: "CompositeFK");

            migrationBuilder.CreateIndex(
                name: "IX_TradeComposites_SummaryId",
                table: "TradeComposites",
                column: "SummaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TradeComposites_TradeElements_SummaryId",
                table: "TradeComposites",
                column: "SummaryId",
                principalTable: "TradeElements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TradeElements_TradeComposites_CompositeFK",
                table: "TradeElements",
                column: "CompositeFK",
                principalTable: "TradeComposites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeComposites_TradeElements_SummaryId",
                table: "TradeComposites");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeElements_TradeComposites_CompositeFK",
                table: "TradeElements");

            migrationBuilder.DropIndex(
                name: "IX_TradeElements_CompositeFK",
                table: "TradeElements");

            migrationBuilder.DropIndex(
                name: "IX_TradeComposites_SummaryId",
                table: "TradeComposites");

            migrationBuilder.DropColumn(
                name: "SummaryId",
                table: "TradeComposites");

            migrationBuilder.AddColumn<int>(
                name: "CompositeRefId",
                table: "TradeElements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TradeElements_CompositeFK",
                table: "TradeElements",
                column: "CompositeFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeElements_CompositeRefId",
                table: "TradeElements",
                column: "CompositeRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_TradeElements_TradeComposites_CompositeFK",
                table: "TradeElements",
                column: "CompositeFK",
                principalTable: "TradeComposites",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeElements_TradeComposites_CompositeRefId",
                table: "TradeElements",
                column: "CompositeRefId",
                principalTable: "TradeComposites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
