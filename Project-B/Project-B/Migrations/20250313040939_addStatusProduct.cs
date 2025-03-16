using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_B.Migrations
{
    /// <inheritdoc />
    public partial class addStatusProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressType",
                table: "AddressModel");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AddressModel",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AddressModel");

            migrationBuilder.AddColumn<int>(
                name: "AddressType",
                table: "AddressModel",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }
    }
}
