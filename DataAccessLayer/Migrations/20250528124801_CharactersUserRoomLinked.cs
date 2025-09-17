using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class CharactersUserRoomLinked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdApplicationUser",
                table: "Characters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "IdRoom",
                table: "Characters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IdApplicationUser",
                table: "Characters",
                column: "IdApplicationUser");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IdRoom",
                table: "Characters",
                column: "IdRoom");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_AspNetUsers_IdApplicationUser",
                table: "Characters",
                column: "IdApplicationUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Rooms_IdRoom",
                table: "Characters",
                column: "IdRoom",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_AspNetUsers_IdApplicationUser",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Rooms_IdRoom",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_IdApplicationUser",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_IdRoom",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IdApplicationUser",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IdRoom",
                table: "Characters");
        }
    }
}
