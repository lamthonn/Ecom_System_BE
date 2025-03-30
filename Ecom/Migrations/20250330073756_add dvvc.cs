using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Migrations
{
    /// <inheritdoc />
    public partial class adddvvc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "dvvc_id",
                table: "account",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "dvvcid",
                table: "account",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_account_dvvcid",
                table: "account",
                column: "dvvcid");

            migrationBuilder.AddForeignKey(
                name: "FK_account_dvvc_dvvcid",
                table: "account",
                column: "dvvcid",
                principalTable: "dvvc",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_account_dvvc_dvvcid",
                table: "account");

            migrationBuilder.DropIndex(
                name: "IX_account_dvvcid",
                table: "account");

            migrationBuilder.DropColumn(
                name: "dvvc_id",
                table: "account");

            migrationBuilder.DropColumn(
                name: "dvvcid",
                table: "account");
        }
    }
}
