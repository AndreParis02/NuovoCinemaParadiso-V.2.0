using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NuovoCinemaParadiso.Migrations
{
    /// <inheritdoc />
    public partial class modificaProiezione4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "attivo",
                table: "Proiezioni",
                newName: "Attivo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Attivo",
                table: "Proiezioni",
                newName: "attivo");
        }
    }
}
