using System;
using Microsoft.EntityFrameworkCore.Migrations;
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

            migrationBuilder.CreateTable(
                name: "d_organizations_types",
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
                    table.PrimaryKey("PK_d_organizations_types", x => x.id);
                    table.UniqueConstraint("AK_d_organizations_types_alias", x => x.alias);
                },
                comment: "Типы организаций");

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
                        name: "FK_r_organizations_d_organizations_types_type_id",
                        column: x => x.type_id,
                        principalSchema: "insania_politics",
                        principalTable: "d_organizations_types",
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
                name: "d_countries",
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
                    table.PrimaryKey("PK_d_countries", x => x.id);
                    table.UniqueConstraint("AK_d_countries_alias", x => x.alias);
                    table.UniqueConstraint("AK_d_countries_color", x => x.color);
                    table.ForeignKey(
                        name: "FK_d_countries_r_organizations_organization_id",
                        column: x => x.organization_id,
                        principalSchema: "insania_politics",
                        principalTable: "r_organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Страны");

            migrationBuilder.CreateIndex(
                name: "IX_d_countries_organization_id",
                schema: "insania_politics",
                table: "d_countries",
                column: "organization_id");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "d_countries",
                schema: "insania_politics");

            migrationBuilder.DropTable(
                name: "r_organizations",
                schema: "insania_politics");

            migrationBuilder.DropTable(
                name: "d_organizations_types",
                schema: "insania_politics");
        }
    }
}
