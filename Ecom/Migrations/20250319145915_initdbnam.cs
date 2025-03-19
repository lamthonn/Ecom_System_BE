using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class initdbnam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_anh_san_pham_san_pham_san_PhamId",
                table: "anh_san_pham");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_don_hang_san_pham_San_phamId",
                table: "chi_tiet_don_hang");

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
                name: "FK_thong_bao_account_accountId",
                table: "thong_bao");

            migrationBuilder.RenameColumn(
                name: "accountId",
                table: "thong_bao",
                newName: "accountid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "thong_bao",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_thong_bao_accountId",
                table: "thong_bao",
                newName: "IX_thong_bao_accountid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "san_pham",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "gio_hang",
                newName: "Accountid");

            migrationBuilder.RenameIndex(
                name: "IX_gio_hang_AccountId",
                table: "gio_hang",
                newName: "IX_gio_hang_Accountid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "dvvc",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "DvvcId",
                table: "don_hang",
                newName: "Dvvcid");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "don_hang",
                newName: "Accountid");

            migrationBuilder.RenameIndex(
                name: "IX_don_hang_DvvcId",
                table: "don_hang",
                newName: "IX_don_hang_Dvvcid");

            migrationBuilder.RenameIndex(
                name: "IX_don_hang_AccountId",
                table: "don_hang",
                newName: "IX_don_hang_Accountid");

            migrationBuilder.RenameColumn(
                name: "San_PhamId",
                table: "danh_gia",
                newName: "San_Phamid");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "danh_gia",
                newName: "Accountid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "danh_gia",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_danh_gia_San_PhamId",
                table: "danh_gia",
                newName: "IX_danh_gia_San_Phamid");

            migrationBuilder.RenameIndex(
                name: "IX_danh_gia_AccountId",
                table: "danh_gia",
                newName: "IX_danh_gia_Accountid");

            migrationBuilder.RenameColumn(
                name: "San_PhamId",
                table: "chi_tiet_gio_hang",
                newName: "San_Phamid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "chi_tiet_gio_hang",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_chi_tiet_gio_hang_San_PhamId",
                table: "chi_tiet_gio_hang",
                newName: "IX_chi_tiet_gio_hang_San_Phamid");

            migrationBuilder.RenameColumn(
                name: "San_phamId",
                table: "chi_tiet_don_hang",
                newName: "San_phamid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "chi_tiet_don_hang",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_chi_tiet_don_hang_San_phamId",
                table: "chi_tiet_don_hang",
                newName: "IX_chi_tiet_don_hang_San_phamid");

            migrationBuilder.RenameColumn(
                name: "san_PhamId",
                table: "anh_san_pham",
                newName: "san_Phamid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "anh_san_pham",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_anh_san_pham_san_PhamId",
                table: "anh_san_pham",
                newName: "IX_anh_san_pham_san_Phamid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "account",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_anh_san_pham_san_pham_san_Phamid",
                table: "anh_san_pham",
                column: "san_Phamid",
                principalTable: "san_pham",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_don_hang_san_pham_San_phamid",
                table: "chi_tiet_don_hang",
                column: "San_phamid",
                principalTable: "san_pham",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_gio_hang_san_pham_San_Phamid",
                table: "chi_tiet_gio_hang",
                column: "San_Phamid",
                principalTable: "san_pham",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_danh_gia_account_Accountid",
                table: "danh_gia",
                column: "Accountid",
                principalTable: "account",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_danh_gia_san_pham_San_Phamid",
                table: "danh_gia",
                column: "San_Phamid",
                principalTable: "san_pham",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_don_hang_account_Accountid",
                table: "don_hang",
                column: "Accountid",
                principalTable: "account",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_don_hang_dvvc_Dvvcid",
                table: "don_hang",
                column: "Dvvcid",
                principalTable: "dvvc",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_gio_hang_account_Accountid",
                table: "gio_hang",
                column: "Accountid",
                principalTable: "account",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_thong_bao_account_accountid",
                table: "thong_bao",
                column: "accountid",
                principalTable: "account",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_anh_san_pham_san_pham_san_Phamid",
                table: "anh_san_pham");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_don_hang_san_pham_San_phamid",
                table: "chi_tiet_don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_chi_tiet_gio_hang_san_pham_San_Phamid",
                table: "chi_tiet_gio_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_danh_gia_account_Accountid",
                table: "danh_gia");

            migrationBuilder.DropForeignKey(
                name: "FK_danh_gia_san_pham_San_Phamid",
                table: "danh_gia");

            migrationBuilder.DropForeignKey(
                name: "FK_don_hang_account_Accountid",
                table: "don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_don_hang_dvvc_Dvvcid",
                table: "don_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_gio_hang_account_Accountid",
                table: "gio_hang");

            migrationBuilder.DropForeignKey(
                name: "FK_thong_bao_account_accountid",
                table: "thong_bao");

            migrationBuilder.RenameColumn(
                name: "accountid",
                table: "thong_bao",
                newName: "accountId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "thong_bao",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_thong_bao_accountid",
                table: "thong_bao",
                newName: "IX_thong_bao_accountId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "san_pham",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Accountid",
                table: "gio_hang",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_gio_hang_Accountid",
                table: "gio_hang",
                newName: "IX_gio_hang_AccountId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "dvvc",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Dvvcid",
                table: "don_hang",
                newName: "DvvcId");

            migrationBuilder.RenameColumn(
                name: "Accountid",
                table: "don_hang",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_don_hang_Dvvcid",
                table: "don_hang",
                newName: "IX_don_hang_DvvcId");

            migrationBuilder.RenameIndex(
                name: "IX_don_hang_Accountid",
                table: "don_hang",
                newName: "IX_don_hang_AccountId");

            migrationBuilder.RenameColumn(
                name: "San_Phamid",
                table: "danh_gia",
                newName: "San_PhamId");

            migrationBuilder.RenameColumn(
                name: "Accountid",
                table: "danh_gia",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "danh_gia",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_danh_gia_San_Phamid",
                table: "danh_gia",
                newName: "IX_danh_gia_San_PhamId");

            migrationBuilder.RenameIndex(
                name: "IX_danh_gia_Accountid",
                table: "danh_gia",
                newName: "IX_danh_gia_AccountId");

            migrationBuilder.RenameColumn(
                name: "San_Phamid",
                table: "chi_tiet_gio_hang",
                newName: "San_PhamId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "chi_tiet_gio_hang",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_chi_tiet_gio_hang_San_Phamid",
                table: "chi_tiet_gio_hang",
                newName: "IX_chi_tiet_gio_hang_San_PhamId");

            migrationBuilder.RenameColumn(
                name: "San_phamid",
                table: "chi_tiet_don_hang",
                newName: "San_phamId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "chi_tiet_don_hang",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_chi_tiet_don_hang_San_phamid",
                table: "chi_tiet_don_hang",
                newName: "IX_chi_tiet_don_hang_San_phamId");

            migrationBuilder.RenameColumn(
                name: "san_Phamid",
                table: "anh_san_pham",
                newName: "san_PhamId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "anh_san_pham",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_anh_san_pham_san_Phamid",
                table: "anh_san_pham",
                newName: "IX_anh_san_pham_san_PhamId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "account",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_anh_san_pham_san_pham_san_PhamId",
                table: "anh_san_pham",
                column: "san_PhamId",
                principalTable: "san_pham",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_chi_tiet_don_hang_san_pham_San_phamId",
                table: "chi_tiet_don_hang",
                column: "San_phamId",
                principalTable: "san_pham",
                principalColumn: "Id");

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
                name: "FK_thong_bao_account_accountId",
                table: "thong_bao",
                column: "accountId",
                principalTable: "account",
                principalColumn: "Id");
        }
    }
}
