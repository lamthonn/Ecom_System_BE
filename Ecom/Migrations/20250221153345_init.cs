using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tai_khoan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mat_khau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ngay_sinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dia_chi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gioi_tinh = table.Column<bool>(type: "bit", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trang_thai = table.Column<bool>(type: "bit", nullable: true),
                    so_dien_thoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_super_admin = table.Column<bool>(type: "bit", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "danh_muc",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_danh_muc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ten_danh_muc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_danh_muc", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dvvc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dvvc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "gio_hang",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gio_hang", x => x.id);
                    table.ForeignKey(
                        name: "FK_gio_hang_account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "thong_bao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nguoi_dung_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    noi_dung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nhom = table.Column<int>(type: "int", nullable: false),
                    trang_thai = table.Column<bool>(type: "bit", nullable: false),
                    thoi_gian_doc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    accountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thong_bao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_thong_bao_account_accountId",
                        column: x => x.accountId,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "san_pham",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    danh_muc_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_san_pham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ten_san_pham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xuat_xu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    luot_ban = table.Column<int>(type: "int", nullable: true),
                    sku = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mau_sac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    danh_Mucid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_san_pham", x => x.Id);
                    table.ForeignKey(
                        name: "FK_san_pham_danh_muc_danh_Mucid",
                        column: x => x.danh_Mucid,
                        principalTable: "danh_muc",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "don_hang",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    dvvc_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_don_hang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    ngay_mua = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tong_tien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    thanh_tien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DvvcId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_don_hang", x => x.id);
                    table.ForeignKey(
                        name: "FK_don_hang_account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_don_hang_dvvc_DvvcId",
                        column: x => x.DvvcId,
                        principalTable: "dvvc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "anh_san_pham",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    duong_dan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ma_san_pham = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    san_pham_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    san_PhamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anh_san_pham", x => x.Id);
                    table.ForeignKey(
                        name: "FK_anh_san_pham_san_pham_san_PhamId",
                        column: x => x.san_PhamId,
                        principalTable: "san_pham",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chi_tiet_gio_hang",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    gio_hang_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    san_pham_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gio_Hangid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    San_PhamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chi_tiet_gio_hang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_chi_tiet_gio_hang_gio_hang_Gio_Hangid",
                        column: x => x.Gio_Hangid,
                        principalTable: "gio_hang",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chi_tiet_gio_hang_san_pham_San_PhamId",
                        column: x => x.San_PhamId,
                        principalTable: "san_pham",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "danh_gia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    san_pham_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    danh_gia_chat_luong = table.Column<int>(type: "int", nullable: false),
                    noi_dung_danh_gia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    San_PhamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_danh_gia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_danh_gia_account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_danh_gia_san_pham_San_PhamId",
                        column: x => x.San_PhamId,
                        principalTable: "san_pham",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chi_tiet_don_hang",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    don_hang_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    san_pham_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    thanh_tien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    so_luong = table.Column<int>(type: "int", nullable: true),
                    Don_Hangid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    San_phamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chi_tiet_don_hang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_chi_tiet_don_hang_don_hang_Don_Hangid",
                        column: x => x.Don_Hangid,
                        principalTable: "don_hang",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chi_tiet_don_hang_san_pham_San_phamId",
                        column: x => x.San_phamId,
                        principalTable: "san_pham",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_anh_san_pham_san_PhamId",
                table: "anh_san_pham",
                column: "san_PhamId");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_don_hang_Don_Hangid",
                table: "chi_tiet_don_hang",
                column: "Don_Hangid");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_don_hang_San_phamId",
                table: "chi_tiet_don_hang",
                column: "San_phamId");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_gio_hang_Gio_Hangid",
                table: "chi_tiet_gio_hang",
                column: "Gio_Hangid");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_gio_hang_San_PhamId",
                table: "chi_tiet_gio_hang",
                column: "San_PhamId");

            migrationBuilder.CreateIndex(
                name: "IX_danh_gia_AccountId",
                table: "danh_gia",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_danh_gia_San_PhamId",
                table: "danh_gia",
                column: "San_PhamId");

            migrationBuilder.CreateIndex(
                name: "IX_don_hang_AccountId",
                table: "don_hang",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_don_hang_DvvcId",
                table: "don_hang",
                column: "DvvcId");

            migrationBuilder.CreateIndex(
                name: "IX_gio_hang_AccountId",
                table: "gio_hang",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_san_pham_danh_Mucid",
                table: "san_pham",
                column: "danh_Mucid");

            migrationBuilder.CreateIndex(
                name: "IX_thong_bao_accountId",
                table: "thong_bao",
                column: "accountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anh_san_pham");

            migrationBuilder.DropTable(
                name: "chi_tiet_don_hang");

            migrationBuilder.DropTable(
                name: "chi_tiet_gio_hang");

            migrationBuilder.DropTable(
                name: "danh_gia");

            migrationBuilder.DropTable(
                name: "thong_bao");

            migrationBuilder.DropTable(
                name: "don_hang");

            migrationBuilder.DropTable(
                name: "gio_hang");

            migrationBuilder.DropTable(
                name: "san_pham");

            migrationBuilder.DropTable(
                name: "dvvc");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "danh_muc");
        }
    }
}
