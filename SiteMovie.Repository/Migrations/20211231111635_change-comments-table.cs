using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteMovie.Repository.Migrations
{
    public partial class changecommentstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "MovieComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MovieComments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_MovieComments_ApplicationUserId",
                table: "MovieComments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieComments_AspNetUsers_ApplicationUserId",
                table: "MovieComments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieComments_AspNetUsers_ApplicationUserId",
                table: "MovieComments");

            migrationBuilder.DropIndex(
                name: "IX_MovieComments_ApplicationUserId",
                table: "MovieComments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "MovieComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MovieComments");
        }
    }
}
