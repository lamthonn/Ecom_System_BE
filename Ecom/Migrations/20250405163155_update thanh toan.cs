using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class updatethanhtoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "lich_su_giao_dich",
                newName: "id");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "lich_su_giao_dich",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "stripeSessionId",
                table: "lich_su_giao_dich",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "lich_su_giao_dich");

            migrationBuilder.DropColumn(
                name: "stripeSessionId",
                table: "lich_su_giao_dich");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "lich_su_giao_dich",
                newName: "Id");
        }
    }
}
