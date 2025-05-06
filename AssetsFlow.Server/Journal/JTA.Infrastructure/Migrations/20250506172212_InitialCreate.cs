using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HsR.Journal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SavedSectors = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ComponentType = table.Column<string>(type: "text", nullable: false),
                    SectorRelevance = table.Column<bool>(type: "boolean", nullable: false),
                    UnitPriceRelevance = table.Column<string>(type: "text", nullable: true),
                    TotalCostRelevance = table.Column<string>(type: "text", nullable: true),
                    IsRelevantForTradeOverview = table.Column<bool>(type: "boolean", nullable: false),
                    IsRelevantForLocalOverview = table.Column<bool>(type: "boolean", nullable: false),
                    Restrictions = table.Column<string>(type: "jsonb", nullable: true),
                    ContentWrapper_ContentValue = table.Column<string>(type: "text", nullable: true),
                    ContentWrapper_ChangeNote = table.Column<string>(type: "text", nullable: true),
                    ContentWrapper_DataElementFK = table.Column<int>(type: "integer", nullable: true),
                    TradeElementFK = table.Column<int>(type: "integer", nullable: false),
                    CompositeFK = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entries_History",
                columns: table => new
                {
                    DataElementId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentValue = table.Column<string>(type: "text", nullable: false),
                    ChangeNote = table.Column<string>(type: "text", nullable: true),
                    DataElementFK = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries_History", x => new { x.DataElementId, x.Id });
                    table.ForeignKey(
                        name: "FK_Entries_History_Entries_DataElementId",
                        column: x => x.DataElementId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeComposites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SummaryId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeComposites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TradeActionType = table.Column<string>(type: "text", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompositeFK = table.Column<int>(type: "integer", nullable: false),
                    TradeElementType = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    IsInterim = table.Column<bool>(type: "boolean", nullable: true),
                    CompositeRefId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeElement_TradeComposites_CompositeFK",
                        column: x => x.CompositeFK,
                        principalTable: "TradeComposites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeElement_TradeComposites_CompositeRefId",
                        column: x => x.CompositeRefId,
                        principalTable: "TradeComposites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_CompositeFK",
                table: "Entries",
                column: "CompositeFK");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_TradeElementFK",
                table: "Entries",
                column: "TradeElementFK");

            migrationBuilder.CreateIndex(
                name: "IX_TradeComposites_SummaryId",
                table: "TradeComposites",
                column: "SummaryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeElement_CompositeFK",
                table: "TradeElement",
                column: "CompositeFK");

            migrationBuilder.CreateIndex(
                name: "IX_TradeElement_CompositeRefId",
                table: "TradeElement",
                column: "CompositeRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_TradeComposites_CompositeFK",
                table: "Entries",
                column: "CompositeFK",
                principalTable: "TradeComposites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_TradeElement_TradeElementFK",
                table: "Entries",
                column: "TradeElementFK",
                principalTable: "TradeElement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeComposites_TradeElement_SummaryId",
                table: "TradeComposites",
                column: "SummaryId",
                principalTable: "TradeElement",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeElement_TradeComposites_CompositeFK",
                table: "TradeElement");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeElement_TradeComposites_CompositeRefId",
                table: "TradeElement");

            migrationBuilder.DropTable(
                name: "Entries_History");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "TradeComposites");

            migrationBuilder.DropTable(
                name: "TradeElement");
        }
    }
}
