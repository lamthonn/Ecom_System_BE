using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class addtablema_giam_giachi_tiet_phieu_nhapphieu_nhap_kho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ma_gia_gia",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    giam_gia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    so_luong = table.Column<int>(type: "int", nullable: false),
                    bat_dau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ket_thuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ma_gia_gia", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "phieu_nhap_kho",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngay_du_kien = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_het_han = table.Column<DateTime>(type: "datetime2", nullable: true),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    nha_cung_cap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thanh_tien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ghi_chu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phieu_nhap_kho", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chi_tiet_phieu_nhap_kho",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    phieu_nhap_kho_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    san_pham_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_san_pham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    so_luong = table.Column<int>(type: "int", nullable: false),
                    don_gia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    sku = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phieu_nhap_khoid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    san_phamid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chi_tiet_phieu_nhap_kho", x => x.id);
                    table.ForeignKey(
                        name: "FK_chi_tiet_phieu_nhap_kho_phieu_nhap_kho_phieu_nhap_khoid",
                        column: x => x.phieu_nhap_khoid,
                        principalTable: "phieu_nhap_kho",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_chi_tiet_phieu_nhap_kho_san_pham_san_phamid",
                        column: x => x.san_phamid,
                        principalTable: "san_pham",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_phieu_nhap_kho_phieu_nhap_khoid",
                table: "chi_tiet_phieu_nhap_kho",
                column: "phieu_nhap_khoid");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_phieu_nhap_kho_san_phamid",
                table: "chi_tiet_phieu_nhap_kho",
                column: "san_phamid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chi_tiet_phieu_nhap_kho");

            migrationBuilder.DropTable(
                name: "ma_gia_gia");

            migrationBuilder.DropTable(
                name: "phieu_nhap_kho");
        }
    }
}
