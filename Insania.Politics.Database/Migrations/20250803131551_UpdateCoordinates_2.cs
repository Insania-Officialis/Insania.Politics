using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insania.Politics.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCoordinates_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_u_countries_coordinates_coordinate_id_country_id",
                schema: "insania_politics",
                table: "u_countries_coordinates");

            migrationBuilder.AlterColumn<long>(
                name: "coordinate_id",
                schema: "insania_politics",
                table: "u_countries_coordinates",
                type: "bigint",
                nullable: true,
                comment: "Идентификатор координаты",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Идентификатор координаты");

            migrationBuilder.AddColumn<string>(
                name: "TypeDiscriminator",
                schema: "insania_politics",
                table: "r_coordinates",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeDiscriminator",
                schema: "insania_politics",
                table: "c_coordinates_types",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_u_countries_coordinates_coordinate_id_country_id_date_delet~",
                schema: "insania_politics",
                table: "u_countries_coordinates",
                columns: new[] { "coordinate_id", "country_id", "date_deleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_r_coordinates_type_id",
                schema: "insania_politics",
                table: "r_coordinates",
                column: "type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_r_coordinates_c_coordinates_types_type_id",
                schema: "insania_politics",
                table: "r_coordinates",
                column: "type_id",
                principalSchema: "insania_politics",
                principalTable: "c_coordinates_types",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_u_countries_coordinates_r_coordinates_coordinate_id",
                schema: "insania_politics",
                table: "u_countries_coordinates",
                column: "coordinate_id",
                principalSchema: "insania_politics",
                principalTable: "r_coordinates",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_r_coordinates_c_coordinates_types_type_id",
                schema: "insania_politics",
                table: "r_coordinates");

            migrationBuilder.DropForeignKey(
                name: "FK_u_countries_coordinates_r_coordinates_coordinate_id",
                schema: "insania_politics",
                table: "u_countries_coordinates");

            migrationBuilder.DropIndex(
                name: "IX_u_countries_coordinates_coordinate_id_country_id_date_delet~",
                schema: "insania_politics",
                table: "u_countries_coordinates");

            migrationBuilder.DropIndex(
                name: "IX_r_coordinates_type_id",
                schema: "insania_politics",
                table: "r_coordinates");

            migrationBuilder.DropColumn(
                name: "TypeDiscriminator",
                schema: "insania_politics",
                table: "r_coordinates");

            migrationBuilder.DropColumn(
                name: "TypeDiscriminator",
                schema: "insania_politics",
                table: "c_coordinates_types");

            migrationBuilder.AlterColumn<long>(
                name: "coordinate_id",
                schema: "insania_politics",
                table: "u_countries_coordinates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "Идентификатор координаты",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Идентификатор координаты");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_u_countries_coordinates_coordinate_id_country_id",
                schema: "insania_politics",
                table: "u_countries_coordinates",
                columns: new[] { "coordinate_id", "country_id" });
        }
    }
}
