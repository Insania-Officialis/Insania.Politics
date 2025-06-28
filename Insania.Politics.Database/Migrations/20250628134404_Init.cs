using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Politics.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "insania_politics");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "c_coordinates_types",
                schema: "insania_politics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Псевдоним")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c_coordinates_types", x => x.id);
                    table.UniqueConstraint("AK_c_coordinates_types_alias", x => x.alias);
                },
                comment: "Типы координат политики");

            migrationBuilder.CreateTable(
                name: "c_organizations_types",
                schema: "insania_politics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Псевдоним")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c_organizations_types", x => x.id);
                    table.UniqueConstraint("AK_c_organizations_types_alias", x => x.alias);
                },
                comment: "Типы организаций");

            migrationBuilder.CreateTable(
                name: "r_coordinates",
                schema: "insania_politics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи"),
                    polygon = table.Column<Polygon>(type: "geometry", nullable: false, comment: "Полигон (массив координат)"),
                    type_id = table.Column<long>(type: "bigint", nullable: true, comment: "Идентификатор типа координаты")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r_coordinates", x => x.id);
                },
                comment: "Координаты политики");

            migrationBuilder.CreateTable(
                name: "r_organizations",
                schema: "insania_politics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    type_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор типа"),
                    parent_id = table.Column<long>(type: "bigint", nullable: true, comment: "Идентификатор родителя"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r_organizations", x => x.id);
                    table.UniqueConstraint("AK_r_organizations_name", x => x.name);
                    table.ForeignKey(
                        name: "FK_r_organizations_c_organizations_types_type_id",
                        column: x => x.type_id,
                        principalSchema: "insania_politics",
                        principalTable: "c_organizations_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_r_organizations_r_organizations_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "insania_politics",
                        principalTable: "r_organizations",
                        principalColumn: "id");
                },
                comment: "Организации");

            migrationBuilder.CreateTable(
                name: "c_countries",
                schema: "insania_politics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false, comment: "Описание"),
                    language_for_names = table.Column<string>(type: "text", nullable: false, comment: "Язык для названий"),
                    color = table.Column<string>(type: "text", nullable: false, comment: "Цвет на карте"),
                    organization_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор организации"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Псевдоним")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c_countries", x => x.id);
                    table.UniqueConstraint("AK_c_countries_alias", x => x.alias);
                    table.UniqueConstraint("AK_c_countries_color", x => x.color);
                    table.ForeignKey(
                        name: "FK_c_countries_r_organizations_organization_id",
                        column: x => x.organization_id,
                        principalSchema: "insania_politics",
                        principalTable: "r_organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Страны");

            migrationBuilder.CreateTable(
                name: "u_countries_coordinates",
                schema: "insania_politics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор страны"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи"),
                    center = table.Column<Point>(type: "geometry", nullable: false, comment: "Координаты точки центра сущности"),
                    zoom = table.Column<int>(type: "integer", nullable: false, comment: "Коэффициент масштаба отображения сущности"),
                    coordinate_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор координаты")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_u_countries_coordinates", x => x.id);
                    table.UniqueConstraint("AK_u_countries_coordinates_coordinate_id_country_id", x => new { x.coordinate_id, x.country_id });
                    table.ForeignKey(
                        name: "FK_u_countries_coordinates_c_countries_country_id",
                        column: x => x.country_id,
                        principalSchema: "insania_politics",
                        principalTable: "c_countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Координаты стран");

            migrationBuilder.CreateIndex(
                name: "IX_c_countries_organization_id",
                schema: "insania_politics",
                table: "c_countries",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_r_coordinates_polygon",
                schema: "insania_politics",
                table: "r_coordinates",
                column: "polygon")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "IX_r_organizations_parent_id",
                schema: "insania_politics",
                table: "r_organizations",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_r_organizations_type_id",
                schema: "insania_politics",
                table: "r_organizations",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_u_countries_coordinates_country_id",
                schema: "insania_politics",
                table: "u_countries_coordinates",
                column: "country_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "c_coordinates_types",
                schema: "insania_politics");

            migrationBuilder.DropTable(
                name: "r_coordinates",
                schema: "insania_politics");

            migrationBuilder.DropTable(
                name: "u_countries_coordinates",
                schema: "insania_politics");

            migrationBuilder.DropTable(
                name: "c_countries",
                schema: "insania_politics");

            migrationBuilder.DropTable(
                name: "r_organizations",
                schema: "insania_politics");

            migrationBuilder.DropTable(
                name: "c_organizations_types",
                schema: "insania_politics");
        }
    }
}
