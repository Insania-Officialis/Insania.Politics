using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Politics.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddLocality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "c_districts",
                schema: "insania_politics");

            migrationBuilder.AddColumn<long>(
                name: "organization_id",
                schema: "insania_politics",
                table: "c_domains",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "Идентификатор организации");

            migrationBuilder.CreateTable(
                name: "c_areas",
                schema: "insania_politics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false, comment: "Описание"),
                    color = table.Column<string>(type: "text", nullable: false, comment: "Цвет на карте"),
                    country_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор страны"),
                    region_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор региона"),
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
                    table.PrimaryKey("PK_c_areas", x => x.id);
                    table.UniqueConstraint("AK_c_areas_alias", x => x.alias);
                    table.UniqueConstraint("AK_c_areas_color", x => x.color);
                    table.ForeignKey(
                        name: "FK_c_areas_c_countries_country_id",
                        column: x => x.country_id,
                        principalSchema: "insania_politics",
                        principalTable: "c_countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_c_areas_c_regions_region_id",
                        column: x => x.region_id,
                        principalSchema: "insania_politics",
                        principalTable: "c_regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Области");

            migrationBuilder.CreateTable(
                name: "c_localities",
                schema: "insania_politics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false, comment: "Описание"),
                    area_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор области"),
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
                    table.PrimaryKey("PK_c_localities", x => x.id);
                    table.UniqueConstraint("AK_c_localities_alias", x => x.alias);
                    table.ForeignKey(
                        name: "FK_c_localities_c_areas_area_id",
                        column: x => x.area_id,
                        principalSchema: "insania_politics",
                        principalTable: "c_areas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Населённые пункты");

            migrationBuilder.CreateIndex(
                name: "IX_c_domains_organization_id",
                schema: "insania_politics",
                table: "c_domains",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_c_areas_country_id",
                schema: "insania_politics",
                table: "c_areas",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_c_areas_region_id",
                schema: "insania_politics",
                table: "c_areas",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "IX_c_localities_area_id",
                schema: "insania_politics",
                table: "c_localities",
                column: "area_id");

            migrationBuilder.AddForeignKey(
                name: "FK_c_domains_r_organizations_organization_id",
                schema: "insania_politics",
                table: "c_domains",
                column: "organization_id",
                principalSchema: "insania_politics",
                principalTable: "r_organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_c_domains_r_organizations_organization_id",
                schema: "insania_politics",
                table: "c_domains");

            migrationBuilder.DropTable(
                name: "c_localities",
                schema: "insania_politics");

            migrationBuilder.DropTable(
                name: "c_areas",
                schema: "insania_politics");

            migrationBuilder.DropIndex(
                name: "IX_c_domains_organization_id",
                schema: "insania_politics",
                table: "c_domains");

            migrationBuilder.DropColumn(
                name: "organization_id",
                schema: "insania_politics",
                table: "c_domains");

            migrationBuilder.CreateTable(
                name: "c_districts",
                schema: "insania_politics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Псевдоним"),
                    color = table.Column<string>(type: "text", nullable: false, comment: "Цвет на карте"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    description = table.Column<string>(type: "text", nullable: false, comment: "Описание"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c_districts", x => x.id);
                    table.UniqueConstraint("AK_c_districts_alias", x => x.alias);
                    table.UniqueConstraint("AK_c_districts_color", x => x.color);
                },
                comment: "Области");
        }
    }
}
