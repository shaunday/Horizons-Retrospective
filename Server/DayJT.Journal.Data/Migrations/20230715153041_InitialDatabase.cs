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
                name: "AllTradeComposites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllTradeComposites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllTradeComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TradeActionType = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    TradePositionCompositeRefId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllTradeComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllTradeComponents_AllTradeComposites_TradePositionComposit~",
                        column: x => x.TradePositionCompositeRefId,
                        principalTable: "AllTradeComposites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllCellContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ChangeNote = table.Column<string>(type: "text", nullable: false),
                    CellRefId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllCellContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllTradeInfoCells",
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
                    table.PrimaryKey("PK_AllTradeInfoCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllTradeInfoCells_AllCellContents_ContentWrapperId",
                        column: x => x.ContentWrapperId,
                        principalTable: "AllCellContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllTradeInfoCells_AllTradeComponents_TradeComponentRefId",
                        column: x => x.TradeComponentRefId,
                        principalTable: "AllTradeComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllCellContents_CellRefId",
                table: "AllCellContents",
                column: "CellRefId");

            migrationBuilder.CreateIndex(
                name: "IX_AllTradeComponents_TradePositionCompositeRefId",
                table: "AllTradeComponents",
                column: "TradePositionCompositeRefId");

            migrationBuilder.CreateIndex(
                name: "IX_AllTradeInfoCells_ContentWrapperId",
                table: "AllTradeInfoCells",
                column: "ContentWrapperId");

            migrationBuilder.CreateIndex(
                name: "IX_AllTradeInfoCells_TradeComponentRefId",
                table: "AllTradeInfoCells",
                column: "TradeComponentRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllCellContents_AllTradeInfoCells_CellRefId",
                table: "AllCellContents",
                column: "CellRefId",
                principalTable: "AllTradeInfoCells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllCellContents_AllTradeInfoCells_CellRefId",
                table: "AllCellContents");

            migrationBuilder.DropTable(
                name: "AllTradeInfoCells");

            migrationBuilder.DropTable(
                name: "AllCellContents");

            migrationBuilder.DropTable(
                name: "AllTradeComponents");

            migrationBuilder.DropTable(
                name: "AllTradeComposites");
        }
    }
}
