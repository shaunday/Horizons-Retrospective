using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HsR.Journal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "TradeComposites");

            migrationBuilder.DropColumn(
                name: "OpenedAt",
                table: "TradeComposites");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "UserData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiry",
                table: "UserData",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiry",
                table: "UserData");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedAt",
                table: "TradeComposites",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OpenedAt",
                table: "TradeComposites",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
