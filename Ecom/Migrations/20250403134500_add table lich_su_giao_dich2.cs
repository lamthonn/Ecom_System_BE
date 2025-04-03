using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class addtablelich_su_giao_dich2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lich_su_giao_dich",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ngay_giao_dich = table.Column<DateTime>(type: "datetime2", nullable: true),
                    phuong_thuc_giao_dich = table.Column<int>(type: "int", nullable: true),
                    giao_dich = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    loai_giao_dich = table.Column<int>(type: "int", nullable: true),
                    so_du = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lich_su_giao_dich", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lich_su_giao_dich");
        }
    }
}
