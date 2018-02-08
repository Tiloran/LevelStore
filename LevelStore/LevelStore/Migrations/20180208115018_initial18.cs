using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LevelStore.Migrations
{
    public partial class initial18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Furniture",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Furniture",
                table: "CartLines",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Furniture",
                table: "CartLines");

            migrationBuilder.AddColumn<int>(
                name: "Furniture",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }
    }
}
