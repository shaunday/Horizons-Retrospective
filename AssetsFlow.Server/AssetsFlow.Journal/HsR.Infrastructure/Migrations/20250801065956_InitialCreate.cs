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
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SavedSectors = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ComponentType = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "TradeElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TradeActionType = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompositeFK = table.Column<int>(type: "integer", nullable: false),
                    TradeElementType = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    IsInterim = table.Column<bool>(type: "boolean", nullable: true),
                    CompositeRefId = table.Column<int>(type: "integer", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_TradeElements_TradeComposites_CompositeRefId",
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
                name: "IX_Entries_UserId",
                table: "Entries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeComposites_SummaryId",
                table: "TradeComposites",
                column: "SummaryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeComposites_UserId",
                table: "TradeComposites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeElements_CompositeFK",
                table: "TradeElements",
                column: "CompositeFK");

            migrationBuilder.CreateIndex(
                name: "IX_TradeElements_CompositeRefId",
                table: "TradeElements",
                column: "CompositeRefId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeElements_UserId",
                table: "TradeElements",
                column: "UserId");

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
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeElements_TradeComposites_CompositeFK",
                table: "TradeElements");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeElements_TradeComposites_CompositeRefId",
                table: "TradeElements");

            migrationBuilder.DropTable(
                name: "Entries_History");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "TradeComposites");

            migrationBuilder.DropTable(
                name: "TradeElements");
        }
    }
}
