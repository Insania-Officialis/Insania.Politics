using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Politics.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalityLevelAndDependencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "locality_level_id",
                schema: "insania_politics",
                table: "c_localities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "Идентификатор населённого пункта");

            migrationBuilder.AddColumn<long>(
                name: "parent_id",
                schema: "insania_politics",
                table: "c_domains",
                type: "bigint",
                nullable: true,
                comment: "Идентификатор родителя");

            migrationBuilder.AddColumn<long>(
                name: "domain_id",
                schema: "insania_politics",
                table: "c_areas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "Идентификатор владения");

            migrationBuilder.CreateTable(
                name: "c_localities_levels",
                schema: "insania_politics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    min_size = table.Column<int>(type: "integer", nullable: false, comment: "Минимальный размер"),
                    max_size = table.Column<int>(type: "integer", nullable: false, comment: "Максимальный размер"),
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
                    table.PrimaryKey("PK_c_localities_levels", x => x.id);
                    table.UniqueConstraint("AK_c_localities_levels_alias", x => x.alias);
                },
                comment: "Уровни населённых пунктов");

            migrationBuilder.CreateIndex(
                name: "IX_c_localities_locality_level_id",
                schema: "insania_politics",
                table: "c_localities",
                column: "locality_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_c_domains_parent_id",
                schema: "insania_politics",
                table: "c_domains",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_c_areas_domain_id",
                schema: "insania_politics",
                table: "c_areas",
                column: "domain_id");

            migrationBuilder.AddForeignKey(
                name: "FK_c_areas_c_domains_domain_id",
                schema: "insania_politics",
                table: "c_areas",
                column: "domain_id",
                principalSchema: "insania_politics",
                principalTable: "c_domains",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_c_domains_c_domains_parent_id",
                schema: "insania_politics",
                table: "c_domains",
                column: "parent_id",
                principalSchema: "insania_politics",
                principalTable: "c_domains",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_c_localities_c_localities_levels_locality_level_id",
                schema: "insania_politics",
                table: "c_localities",
                column: "locality_level_id",
                principalSchema: "insania_politics",
                principalTable: "c_localities_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_c_areas_c_domains_domain_id",
                schema: "insania_politics",
                table: "c_areas");

            migrationBuilder.DropForeignKey(
                name: "FK_c_domains_c_domains_parent_id",
                schema: "insania_politics",
                table: "c_domains");

            migrationBuilder.DropForeignKey(
                name: "FK_c_localities_c_localities_levels_locality_level_id",
                schema: "insania_politics",
                table: "c_localities");

            migrationBuilder.DropTable(
                name: "c_localities_levels",
                schema: "insania_politics");

            migrationBuilder.DropIndex(
                name: "IX_c_localities_locality_level_id",
                schema: "insania_politics",
                table: "c_localities");

            migrationBuilder.DropIndex(
                name: "IX_c_domains_parent_id",
                schema: "insania_politics",
                table: "c_domains");

            migrationBuilder.DropIndex(
                name: "IX_c_areas_domain_id",
                schema: "insania_politics",
                table: "c_areas");

            migrationBuilder.DropColumn(
                name: "locality_level_id",
                schema: "insania_politics",
                table: "c_localities");

            migrationBuilder.DropColumn(
                name: "parent_id",
                schema: "insania_politics",
                table: "c_domains");

            migrationBuilder.DropColumn(
                name: "domain_id",
                schema: "insania_politics",
                table: "c_areas");
        }
    }
}
