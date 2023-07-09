using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayJTrading.Journal.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OverallTrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverallTrades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeInputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TradeActionType = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    TradePositionCompositeRefId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeInputs_OverallTrades_TradePositionCompositeRefId",
                        column: x => x.TradePositionCompositeRefId,
                        principalTable: "OverallTrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ChangeNote = table.Column<string>(type: "text", nullable: false),
                    CellRefId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeInputComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ComponentType = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    ContentWrapperId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CostRelevance = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    PriceRelevance = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    IsRelevantForOverview = table.Column<bool>(type: "boolean", nullable: false),
                    TradeComponentRefId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeInputComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeInputComponents_ContentModels_ContentWrapperId",
                        column: x => x.ContentWrapperId,
                        principalTable: "ContentModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeInputComponents_TradeInputs_TradeComponentRefId",
                        column: x => x.TradeComponentRefId,
                        principalTable: "TradeInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentModels_CellRefId",
                table: "ContentModels",
                column: "CellRefId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeInputComponents_ContentWrapperId",
                table: "TradeInputComponents",
                column: "ContentWrapperId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeInputComponents_TradeComponentRefId",
                table: "TradeInputComponents",
                column: "TradeComponentRefId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeInputs_TradePositionCompositeRefId",
                table: "TradeInputs",
                column: "TradePositionCompositeRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentModels_TradeInputComponents_CellRefId",
                table: "ContentModels",
                column: "CellRefId",
                principalTable: "TradeInputComponents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentModels_TradeInputComponents_CellRefId",
                table: "ContentModels");

            migrationBuilder.DropTable(
                name: "TradeInputComponents");

            migrationBuilder.DropTable(
                name: "ContentModels");

            migrationBuilder.DropTable(
                name: "TradeInputs");

            migrationBuilder.DropTable(
                name: "OverallTrades");
        }
    }
}
