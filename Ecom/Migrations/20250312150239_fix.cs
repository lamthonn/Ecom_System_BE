using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_anh_san_pham_san_pham_san_PhamId",
                table: "anh_san_pham");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_don_hang_don_hang_Don_Hangid",
                table: "chi_tiet_don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_don_hang_san_pham_San_phamId",
                table: "chi_tiet_don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_gio_hang_gio_hang_Gio_Hangid",
                table: "chi_tiet_gio_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_gio_hang_san_pham_San_PhamId",
                table: "chi_tiet_gio_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_danh_gia_account_AccountId",
                table: "danh_gia");

            migrationBuilder.DropForeignKey(
                name: "FK_danh_gia_san_pham_San_PhamId",
                table: "danh_gia");

            migrationBuilder.DropForeignKey(
                name: "FK_don_hang_account_AccountId",
                table: "don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_don_hang_dvvc_DvvcId",
                table: "don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_gio_hang_account_AccountId",
                table: "gio_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_san_pham_danh_muc_danh_Mucid",
                table: "san_pham");

            migrationBuilder.DropForeignKey(
                name: "FK_thong_bao_account_accountId",
                table: "thong_bao");

            migrationBuilder.AlterColumn<Guid>(
                name: "accountId",
                table: "thong_bao",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "danh_Mucid",
                table: "san_pham",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "gio_hang",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "DvvcId",
                table: "don_hang",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "don_hang",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "San_PhamId",
                table: "danh_gia",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "danh_gia",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "San_PhamId",
                table: "chi_tiet_gio_hang",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Gio_Hangid",
                table: "chi_tiet_gio_hang",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "San_phamId",
                table: "chi_tiet_don_hang",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Don_Hangid",
                table: "chi_tiet_don_hang",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "san_PhamId",
                table: "anh_san_pham",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_anh_san_pham_san_pham_san_PhamId",
                table: "anh_san_pham",
                column: "san_PhamId",
                principalTable: "san_pham",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_don_hang_don_hang_Don_Hangid",
                table: "chi_tiet_don_hang",
                column: "Don_Hangid",
                principalTable: "don_hang",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_don_hang_san_pham_San_phamId",
                table: "chi_tiet_don_hang",
                column: "San_phamId",
                principalTable: "san_pham",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_gio_hang_gio_hang_Gio_Hangid",
                table: "chi_tiet_gio_hang",
                column: "Gio_Hangid",
                principalTable: "gio_hang",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_gio_hang_san_pham_San_PhamId",
                table: "chi_tiet_gio_hang",
                column: "San_PhamId",
                principalTable: "san_pham",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_danh_gia_account_AccountId",
                table: "danh_gia",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_danh_gia_san_pham_San_PhamId",
                table: "danh_gia",
                column: "San_PhamId",
                principalTable: "san_pham",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_don_hang_account_AccountId",
                table: "don_hang",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_don_hang_dvvc_DvvcId",
                table: "don_hang",
                column: "DvvcId",
                principalTable: "dvvc",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_gio_hang_account_AccountId",
                table: "gio_hang",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_san_pham_danh_muc_danh_Mucid",
                table: "san_pham",
                column: "danh_Mucid",
                principalTable: "danh_muc",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_thong_bao_account_accountId",
                table: "thong_bao",
                column: "accountId",
                principalTable: "account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_anh_san_pham_san_pham_san_PhamId",
                table: "anh_san_pham");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_don_hang_don_hang_Don_Hangid",
                table: "chi_tiet_don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_don_hang_san_pham_San_phamId",
                table: "chi_tiet_don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_gio_hang_gio_hang_Gio_Hangid",
                table: "chi_tiet_gio_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_gio_hang_san_pham_San_PhamId",
                table: "chi_tiet_gio_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_danh_gia_account_AccountId",
                table: "danh_gia");

            migrationBuilder.DropForeignKey(
                name: "FK_danh_gia_san_pham_San_PhamId",
                table: "danh_gia");

            migrationBuilder.DropForeignKey(
                name: "FK_don_hang_account_AccountId",
                table: "don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_don_hang_dvvc_DvvcId",
                table: "don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_gio_hang_account_AccountId",
                table: "gio_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_san_pham_danh_muc_danh_Mucid",
                table: "san_pham");

            migrationBuilder.DropForeignKey(
                name: "FK_thong_bao_account_accountId",
                table: "thong_bao");

            migrationBuilder.AlterColumn<Guid>(
                name: "accountId",
                table: "thong_bao",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "danh_Mucid",
                table: "san_pham",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "gio_hang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DvvcId",
                table: "don_hang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "don_hang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "San_PhamId",
                table: "danh_gia",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "danh_gia",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "San_PhamId",
                table: "chi_tiet_gio_hang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Gio_Hangid",
                table: "chi_tiet_gio_hang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "San_phamId",
                table: "chi_tiet_don_hang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Don_Hangid",
                table: "chi_tiet_don_hang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "san_PhamId",
                table: "anh_san_pham",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_anh_san_pham_san_pham_san_PhamId",
                table: "anh_san_pham",
                column: "san_PhamId",
                principalTable: "san_pham",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_don_hang_don_hang_Don_Hangid",
                table: "chi_tiet_don_hang",
                column: "Don_Hangid",
                principalTable: "don_hang",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_don_hang_san_pham_San_phamId",
                table: "chi_tiet_don_hang",
                column: "San_phamId",
                principalTable: "san_pham",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_gio_hang_gio_hang_Gio_Hangid",
                table: "chi_tiet_gio_hang",
                column: "Gio_Hangid",
                principalTable: "gio_hang",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_gio_hang_san_pham_San_PhamId",
                table: "chi_tiet_gio_hang",
                column: "San_PhamId",
                principalTable: "san_pham",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_danh_gia_account_AccountId",
                table: "danh_gia",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_danh_gia_san_pham_San_PhamId",
                table: "danh_gia",
                column: "San_PhamId",
                principalTable: "san_pham",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_don_hang_account_AccountId",
                table: "don_hang",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_don_hang_dvvc_DvvcId",
                table: "don_hang",
                column: "DvvcId",
                principalTable: "dvvc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_gio_hang_account_AccountId",
                table: "gio_hang",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_san_pham_danh_muc_danh_Mucid",
                table: "san_pham",
                column: "danh_Mucid",
                principalTable: "danh_muc",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_thong_bao_account_accountId",
                table: "thong_bao",
                column: "accountId",
                principalTable: "account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
