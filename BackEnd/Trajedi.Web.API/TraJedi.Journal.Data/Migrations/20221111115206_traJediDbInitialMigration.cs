using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraJedi.Journal.Data.Migrations
{
    public partial class traJediDbInitialMigration : Migration
    {
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
                    TradeInputType = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TradeModelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeInputs_OverallTrades_TradeModelId",
                        column: x => x.TradeModelId,
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
                    CostRelevance = table.Column<int>(type: "integer", nullable: false),
                    PriceValueRelevance = table.Column<int>(type: "integer", nullable: false),
                    AttachedToggle = table.Column<bool>(type: "boolean", nullable: false),
                    IsRelevantForOneLineSummation = table.Column<bool>(type: "boolean", nullable: false),
                    TradeInputModelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeInputComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeInputComponents_TradeInputs_TradeInputModelId",
                        column: x => x.TradeInputModelId,
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
                    InputComponentModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    InputComponentModelId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentModels_TradeInputComponents_InputComponentModelId",
                        column: x => x.InputComponentModelId,
                        principalTable: "TradeInputComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentModels_TradeInputComponents_InputComponentModelId1",
                        column: x => x.InputComponentModelId1,
                        principalTable: "TradeInputComponents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentModels_InputComponentModelId",
                table: "ContentModels",
                column: "InputComponentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentModels_InputComponentModelId1",
                table: "ContentModels",
                column: "InputComponentModelId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeInputComponents_TradeInputModelId",
                table: "TradeInputComponents",
                column: "TradeInputModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeInputs_TradeModelId",
                table: "TradeInputs",
                column: "TradeModelId");
        }

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
