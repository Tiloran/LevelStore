using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LevelStore.Migrations
{
    public partial class initial22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShareID",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    ShareId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateOfEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    KoefPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.ShareId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShareID",
                table: "Products",
                column: "ShareID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shares_ShareID",
                table: "Products",
                column: "ShareID",
                principalTable: "Shares",
                principalColumn: "ShareId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shares_ShareID",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Shares");

            migrationBuilder.DropIndex(
                name: "IX_Products_ShareID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShareID",
                table: "Products");
        }
    }
}
