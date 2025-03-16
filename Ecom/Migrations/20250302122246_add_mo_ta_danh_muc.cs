using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class add_mo_ta_danh_muc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mo_ta",
                table: "danh_muc",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mo_ta",
                table: "danh_muc");
        }
    }
}
