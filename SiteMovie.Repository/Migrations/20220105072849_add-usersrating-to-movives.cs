using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteMovie.Repository.Migrations
{
    public partial class addusersratingtomovives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UsersRating",
                table: "Movies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsersRating",
                table: "Movies");
        }
    }
}
