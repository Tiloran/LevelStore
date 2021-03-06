﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LevelStore.Migrations
{
    public partial class initial10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessoriesForBags",
                table: "Products",
                newName: "AccessorieForBagID");

            migrationBuilder.CreateTable(
                name: "AccessoriesForBags",
                columns: table => new
                {
                    AccessorieForBagID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoriesForBags", x => x.AccessorieForBagID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_AccessorieForBagID",
                table: "Products",
                column: "AccessorieForBagID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AccessoriesForBags_AccessorieForBagID",
                table: "Products",
                column: "AccessorieForBagID",
                principalTable: "AccessoriesForBags",
                principalColumn: "AccessorieForBagID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AccessoriesForBags_AccessorieForBagID",
                table: "Products");

            migrationBuilder.DropTable(
                name: "AccessoriesForBags");

            migrationBuilder.DropIndex(
                name: "IX_Products_AccessorieForBagID",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "AccessorieForBagID",
                table: "Products",
                newName: "AccessoriesForBags");
        }
    }
}
