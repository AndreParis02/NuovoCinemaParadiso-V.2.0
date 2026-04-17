using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NuovoCinemaParadiso.Migrations
{
    /// <inheritdoc />
    public partial class terza_migrazione : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proiezioni_AspNetUsers_TurnoId",
                table: "Proiezioni");

            migrationBuilder.AddForeignKey(
                name: "FK_Proiezioni_Turni_TurnoId",
                table: "Proiezioni",
                column: "TurnoId",
                principalTable: "Turni",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proiezioni_Turni_TurnoId",
                table: "Proiezioni");

            migrationBuilder.AddForeignKey(
                name: "FK_Proiezioni_AspNetUsers_TurnoId",
                table: "Proiezioni",
                column: "TurnoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
