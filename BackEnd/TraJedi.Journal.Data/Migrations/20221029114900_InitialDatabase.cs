using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraJedi.Journal.Data.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OverallTrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OverallTradeId = table.Column<Guid>(type: "uuid", nullable: false)
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
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TradeInputType = table.Column<int>(type: "integer", nullable: false),
                    OverallTradeModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeInputs_OverallTrades_OverallTradeModelId",
                        column: x => x.OverallTradeModelId,
                        principalTable: "OverallTrades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContentModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    InputComponentModelId = table.Column<Guid>(type: "uuid", nullable: true)
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
                    Title = table.Column<string>(type: "text", nullable: false),
                    ComponentType = table.Column<int>(type: "integer", nullable: false),
                    ContentWrapperId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CostRelevant = table.Column<int>(type: "integer", nullable: false),
                    PriceValueRelevant = table.Column<int>(type: "integer", nullable: false),
                    AttachedToggle = table.Column<bool>(type: "boolean", nullable: false),
                    RelevantForTradeSummary = table.Column<bool>(type: "boolean", nullable: false),
                    TradeInputModelId = table.Column<Guid>(type: "uuid", nullable: true)
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
                        name: "FK_TradeInputComponents_TradeInputs_TradeInputModelId",
                        column: x => x.TradeInputModelId,
                        principalTable: "TradeInputs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentModels_InputComponentModelId",
                table: "ContentModels",
                column: "InputComponentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeInputComponents_ContentWrapperId",
                table: "TradeInputComponents",
                column: "ContentWrapperId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeInputComponents_TradeInputModelId",
                table: "TradeInputComponents",
                column: "TradeInputModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeInputs_OverallTradeModelId",
                table: "TradeInputs",
                column: "OverallTradeModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentModels_TradeInputComponents_InputComponentModelId",
                table: "ContentModels",
                column: "InputComponentModelId",
                principalTable: "TradeInputComponents",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentModels_TradeInputComponents_InputComponentModelId",
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
