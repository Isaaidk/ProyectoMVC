using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication4.Migrations
{
    /// <inheritdoc />
    public partial class Migracion1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Asignacion",
                columns: table => new
                {
                    IdAsignacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignacion", x => x.IdAsignacion);
                });

            migrationBuilder.CreateTable(
                name: "Alumno",
                columns: table => new
                {
                    IdBanner = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modalidad = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Asistencia = table.Column<bool>(type: "bit", nullable: false),
                    IdAsignacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumno", x => x.IdBanner);
                    table.ForeignKey(
                        name: "FK_Alumno_Asignacion_IdAsignacion",
                        column: x => x.IdAsignacion,
                        principalTable: "Asignacion",
                        principalColumn: "IdAsignacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alumno_IdAsignacion",
                table: "Alumno",
                column: "IdAsignacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alumno");

            migrationBuilder.DropTable(
                name: "Asignacion");
        }
    }
}
