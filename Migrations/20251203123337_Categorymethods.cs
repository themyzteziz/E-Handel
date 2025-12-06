using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Handel.Migrations
{
    /// <inheritdoc />
    public partial class Categorymethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId1",
                table: "Products",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId1",
                table: "Products",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Products");
        }
    }
}
