using Microsoft.EntityFrameworkCore.Migrations;

namespace LevelStore.Migrations
{
    public partial class initial6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_TypeColors_TypeColorID",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "TypeColorID",
                table: "Images",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Images_TypeColors_TypeColorID",
                table: "Images",
                column: "TypeColorID",
                principalTable: "TypeColors",
                principalColumn: "TypeColorID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_TypeColors_TypeColorID",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "TypeColorID",
                table: "Images",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_TypeColors_TypeColorID",
                table: "Images",
                column: "TypeColorID",
                principalTable: "TypeColors",
                principalColumn: "TypeColorID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
