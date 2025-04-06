using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class upddanhgia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "don_hang_id",
                table: "danh_gia",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "don_hangid",
                table: "danh_gia",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ma_don_hang",
                table: "danh_gia",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_danh_gia_don_hangid",
                table: "danh_gia",
                column: "don_hangid");

            migrationBuilder.AddForeignKey(
                name: "FK_danh_gia_don_hang_don_hangid",
                table: "danh_gia",
                column: "don_hangid",
                principalTable: "don_hang",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_danh_gia_don_hang_don_hangid",
                table: "danh_gia");

            migrationBuilder.DropIndex(
                name: "IX_danh_gia_don_hangid",
                table: "danh_gia");

            migrationBuilder.DropColumn(
                name: "don_hang_id",
                table: "danh_gia");

            migrationBuilder.DropColumn(
                name: "don_hangid",
                table: "danh_gia");

            migrationBuilder.DropColumn(
                name: "ma_don_hang",
                table: "danh_gia");
        }
    }
}
