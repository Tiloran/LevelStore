using Microsoft.EntityFrameworkCore.Migrations;

namespace LevelStore.Migrations
{
    public partial class initial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeColorID",
                table: "Images",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_TypeColorID",
                table: "Images",
                column: "TypeColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_TypeColors_TypeColorID",
                table: "Images",
                column: "TypeColorID",
                principalTable: "TypeColors",
                principalColumn: "TypeColorID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_TypeColors_TypeColorID",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_TypeColorID",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "TypeColorID",
                table: "Images");
        }
    }
}
