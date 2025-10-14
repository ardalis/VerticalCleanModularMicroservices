using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderDemo.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GuestUserId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GuestUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_GuestUserId",
                table: "Orders",
                column: "GuestUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_GuestUsers_GuestUserId",
                table: "Orders",
                column: "GuestUserId",
                principalTable: "GuestUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_GuestUsers_GuestUserId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "GuestUsers");

            migrationBuilder.DropIndex(
                name: "IX_Orders_GuestUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GuestUserId",
                table: "Orders");
        }
    }
}
