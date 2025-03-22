using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_B.Migrations
{
    /// <inheritdoc />
    public partial class addcolumniscancel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsCancel",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancel",
                table: "Orders");
        }
    }
}
