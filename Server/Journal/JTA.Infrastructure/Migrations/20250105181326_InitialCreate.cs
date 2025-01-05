using System;
using System.Collections.Generic;
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
                name: "JournalData",
                columns: table => new
                {
                    SavedSectors = table.Column<List<string>>(type: "jsonb", nullable: true),
                    SavedBrokers = table.Column<List<string>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ComponentType = table.Column<string>(type: "text", nullable: false),
                    CostRelevance = table.Column<string>(type: "text", nullable: true),
                    PriceRelevance = table.Column<string>(type: "text", nullable: true),
                    IsRelevantForOverview = table.Column<bool>(type: "boolean", nullable: false),
                    ContentWrapper_ContentValue = table.Column<string>(type: "text", nullable: false),
                    ContentWrapper_ChangeNote = table.Column<string>(type: "text", nullable: true),
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
                    DataElementFK = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentValue = table.Column<string>(type: "text", nullable: false),
                    ChangeNote = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries_History", x => new { x.DataElementFK, x.Id });
                    table.ForeignKey(
                        name: "FK_Entries_History_Entries_DataElementFK",
                        column: x => x.DataElementFK,
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
                    Sectors = table.Column<List<string>>(type: "text[]", nullable: false),
                    SummaryId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeComposites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TradeActionType = table.Column<string>(type: "text", nullable: false),
                    CompositeFK = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeElements_TradeComposites_CompositeFK",
                        column: x => x.CompositeFK,
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
                column: "SummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeElements_CompositeFK",
                table: "TradeElements",
                column: "CompositeFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_TradeComposites_CompositeFK",
                table: "Entries",
                column: "CompositeFK",
                principalTable: "TradeComposites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_TradeElements_TradeElementFK",
                table: "Entries",
                column: "TradeElementFK",
                principalTable: "TradeElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeComposites_TradeElements_SummaryId",
                table: "TradeComposites",
                column: "SummaryId",
                principalTable: "TradeElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeElements_TradeComposites_CompositeFK",
                table: "TradeElements");

            migrationBuilder.DropTable(
                name: "Entries_History");

            migrationBuilder.DropTable(
                name: "JournalData");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "TradeComposites");

            migrationBuilder.DropTable(
                name: "TradeElements");
        }
    }
}
