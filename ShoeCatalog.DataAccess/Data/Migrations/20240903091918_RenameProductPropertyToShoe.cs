using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeCatalog.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductPropertyToShoe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Shoes_ProductId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Carts",
                newName: "ShoeId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                newName: "IX_Carts_ShoeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Shoes_ShoeId",
                table: "Carts",
                column: "ShoeId",
                principalTable: "Shoes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Shoes_ShoeId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ShoeId",
                table: "Carts",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_ShoeId",
                table: "Carts",
                newName: "IX_Carts_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Shoes_ProductId",
                table: "Carts",
                column: "ProductId",
                principalTable: "Shoes",
                principalColumn: "Id");
        }
    }
}
