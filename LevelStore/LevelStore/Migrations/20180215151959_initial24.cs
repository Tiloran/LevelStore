using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LevelStore.Migrations
{
    public partial class initial24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "KoefPriceAfterCheckout",
                table: "CartLines",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAfterCheckout",
                table: "CartLines",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KoefPriceAfterCheckout",
                table: "CartLines");

            migrationBuilder.DropColumn(
                name: "PriceAfterCheckout",
                table: "CartLines");
        }
    }
}
