using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LevelStore.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Color_Products_ProductID",
                table: "Color");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Color",
                table: "Color");

            migrationBuilder.RenameTable(
                name: "Color",
                newName: "Colors");

            migrationBuilder.RenameIndex(
                name: "IX_Color_ProductID",
                table: "Colors",
                newName: "IX_Colors_ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colors",
                table: "Colors",
                column: "ColorID");

            migrationBuilder.CreateTable(
                name: "TypeColors",
                columns: table => new
                {
                    TypeColorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ColorType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeColors", x => x.TypeColorID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colors_TypeColorID",
                table: "Colors",
                column: "TypeColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Products_ProductID",
                table: "Colors",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_TypeColors_TypeColorID",
                table: "Colors",
                column: "TypeColorID",
                principalTable: "TypeColors",
                principalColumn: "TypeColorID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Products_ProductID",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Colors_TypeColors_TypeColorID",
                table: "Colors");

            migrationBuilder.DropTable(
                name: "TypeColors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colors",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Colors_TypeColorID",
                table: "Colors");

            migrationBuilder.RenameTable(
                name: "Colors",
                newName: "Color");

            migrationBuilder.RenameIndex(
                name: "IX_Colors_ProductID",
                table: "Color",
                newName: "IX_Color_ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Color",
                table: "Color",
                column: "ColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Color_Products_ProductID",
                table: "Color",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
