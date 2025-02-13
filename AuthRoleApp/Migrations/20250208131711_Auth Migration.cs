using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuthRoleApp.Migrations
{
    /// <inheritdoc />
    public partial class AuthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    correo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    password_hash = table.Column<byte[]>(type: "bytea", nullable: true),
                    password_salt = table.Column<byte[]>(type: "bytea", nullable: true),
                    token = table.Column<string>(type: "character varying", nullable: true),
                    rol = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    nombre_usuario = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuarios_pkey", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
