using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_B.Migrations
{
    /// <inheritdoc />
    public partial class intitial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductModel_CategoryModel_CategoryId",
                table: "ProductModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductModel",
                table: "ProductModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryModel",
                table: "CategoryModel");

            migrationBuilder.RenameTable(
                name: "ProductModel",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "CategoryModel",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_ProductModel_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "ProductModel");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "CategoryModel");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "ProductModel",
                newName: "IX_ProductModel_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductModel",
                table: "ProductModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryModel",
                table: "CategoryModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModel_CategoryModel_CategoryId",
                table: "ProductModel",
                column: "CategoryId",
                principalTable: "CategoryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
