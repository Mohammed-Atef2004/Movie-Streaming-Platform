using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addwatchedlistandfavourites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Series",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Series",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Movies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Movies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Series_ApplicationUserId",
                table: "Series",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_ApplicationUserId1",
                table: "Series",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ApplicationUserId",
                table: "Movies",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ApplicationUserId1",
                table: "Movies",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_ApplicationUserId",
                table: "Movies",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_ApplicationUserId1",
                table: "Movies",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Series_AspNetUsers_ApplicationUserId",
                table: "Series",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Series_AspNetUsers_ApplicationUserId1",
                table: "Series",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_ApplicationUserId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_ApplicationUserId1",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_AspNetUsers_ApplicationUserId",
                table: "Series");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_AspNetUsers_ApplicationUserId1",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_ApplicationUserId",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_ApplicationUserId1",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ApplicationUserId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ApplicationUserId1",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "AspNetUsers");
        }
    }
}
