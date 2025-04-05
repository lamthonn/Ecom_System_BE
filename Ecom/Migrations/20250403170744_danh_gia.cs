using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class danh_gia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_danh_gia",
                table: "don_hang",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "noi_dung_phan_hoi",
                table: "danh_gia",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_danh_gia",
                table: "don_hang");

            migrationBuilder.DropColumn(
                name: "noi_dung_phan_hoi",
                table: "danh_gia");
        }
    }
}
