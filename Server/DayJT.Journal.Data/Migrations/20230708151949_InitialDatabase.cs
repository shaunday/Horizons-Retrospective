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
                    TradePositionCompositeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeInputs_OverallTrades_TradePositionCompositeId",
                        column: x => x.TradePositionCompositeId,
                        principalTable: "OverallTrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeInputComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ComponentType = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CostRelevance = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    PriceRelevance = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    IsRelevantForOverview = table.Column<bool>(type: "boolean", nullable: false),
                    TradeComponentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeInputComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeInputComponents_TradeInputs_TradeComponentId",
                        column: x => x.TradeComponentId,
                        principalTable: "TradeInputs",
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
                    CellId = table.Column<Guid>(type: "uuid", nullable: false),
                    CellId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentModels_TradeInputComponents_CellId",
                        column: x => x.CellId,
                        principalTable: "TradeInputComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentModels_TradeInputComponents_CellId1",
                        column: x => x.CellId1,
                        principalTable: "TradeInputComponents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentModels_CellId",
                table: "ContentModels",
                column: "CellId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentModels_CellId1",
                table: "ContentModels",
                column: "CellId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeInputComponents_TradeComponentId",
                table: "TradeInputComponents",
                column: "TradeComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeInputs_TradePositionCompositeId",
                table: "TradeInputs",
                column: "TradePositionCompositeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentModels");

            migrationBuilder.DropTable(
                name: "TradeInputComponents");

            migrationBuilder.DropTable(
                name: "TradeInputs");

            migrationBuilder.DropTable(
                name: "OverallTrades");
        }
    }
}
