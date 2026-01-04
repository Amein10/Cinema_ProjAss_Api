using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema_ProjAss_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieDurationMinutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Movies");
        }
    }
}
