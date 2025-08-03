using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insania.Politics.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCoordinates_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "area",
                schema: "insania_politics",
                table: "u_countries_coordinates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                comment: "Площадь сущности");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                schema: "insania_politics",
                table: "c_coordinates_types",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BorderColor",
                schema: "insania_politics",
                table: "c_coordinates_types",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "area",
                schema: "insania_politics",
                table: "u_countries_coordinates");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                schema: "insania_politics",
                table: "c_coordinates_types");

            migrationBuilder.DropColumn(
                name: "BorderColor",
                schema: "insania_politics",
                table: "c_coordinates_types");
        }
    }
}
