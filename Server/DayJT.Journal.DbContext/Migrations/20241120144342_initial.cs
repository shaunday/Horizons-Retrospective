using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DayJTrading.Journal.DbContext.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JournalData",
                columns: table => new
                {
                    SavedSectors = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AllContentRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ChangeNote = table.Column<string>(type: "text", nullable: false),
                    CellRefId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllContentRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ComponentType = table.Column<int>(type: "integer", nullable: false),
                    CostRelevance = table.Column<int>(type: "integer", nullable: false),
                    PriceRelevance = table.Column<int>(type: "integer", nullable: false),
                    IsRelevantForOverview = table.Column<bool>(type: "boolean", nullable: false),
                    ContentWrapperId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    TradeElementRefId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllEntries_AllContentRecords_ContentWrapperId",
                        column: x => x.ContentWrapperId,
                        principalTable: "AllContentRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllTradeComposites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sector = table.Column<string>(type: "text", nullable: false),
                    SummaryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllTradeComposites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllTradeElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TradeActionType = table.Column<int>(type: "integer", nullable: false),
                    TradeCompositeRefId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllTradeElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllTradeElements_AllTradeComposites_TradeCompositeRefId",
                        column: x => x.TradeCompositeRefId,
                        principalTable: "AllTradeComposites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllContentRecords_CellRefId",
                table: "AllContentRecords",
                column: "CellRefId");

            migrationBuilder.CreateIndex(
                name: "IX_AllEntries_ContentWrapperId",
                table: "AllEntries",
                column: "ContentWrapperId");

            migrationBuilder.CreateIndex(
                name: "IX_AllEntries_TradeElementRefId",
                table: "AllEntries",
                column: "TradeElementRefId");

            migrationBuilder.CreateIndex(
                name: "IX_AllTradeComposites_SummaryId",
                table: "AllTradeComposites",
                column: "SummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_AllTradeElements_TradeCompositeRefId",
                table: "AllTradeElements",
                column: "TradeCompositeRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllContentRecords_AllEntries_CellRefId",
                table: "AllContentRecords",
                column: "CellRefId",
                principalTable: "AllEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AllEntries_AllTradeElements_TradeElementRefId",
                table: "AllEntries",
                column: "TradeElementRefId",
                principalTable: "AllTradeElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AllTradeComposites_AllTradeElements_SummaryId",
                table: "AllTradeComposites",
                column: "SummaryId",
                principalTable: "AllTradeElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllContentRecords_AllEntries_CellRefId",
                table: "AllContentRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AllTradeComposites_AllTradeElements_SummaryId",
                table: "AllTradeComposites");

            migrationBuilder.DropTable(
                name: "JournalData");

            migrationBuilder.DropTable(
                name: "AllEntries");

            migrationBuilder.DropTable(
                name: "AllContentRecords");

            migrationBuilder.DropTable(
                name: "AllTradeElements");

            migrationBuilder.DropTable(
                name: "AllTradeComposites");
        }
    }
}
