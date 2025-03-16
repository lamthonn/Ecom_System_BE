using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class add_gia_add_khuyenMai_san_phan__add_so_luong_chi_tiet_gio_hang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "gia",
                table: "san_pham",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "khuyen_mai",
                table: "san_pham",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "so_luong",
                table: "chi_tiet_gio_hang",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gia",
                table: "san_pham");

            migrationBuilder.DropColumn(
                name: "khuyen_mai",
                table: "san_pham");

            migrationBuilder.DropColumn(
                name: "so_luong",
                table: "chi_tiet_gio_hang");
        }
    }
}
