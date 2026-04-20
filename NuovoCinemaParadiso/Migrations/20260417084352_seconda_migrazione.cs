using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NuovoCinemaParadiso.Migrations
{
    /// <inheritdoc />
    public partial class seconda_migrazione : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acquisto_AspNetUsers_UtenteId",
                table: "Acquisto");

            migrationBuilder.DropForeignKey(
                name: "FK_Acquisto_Movie_MovieId",
                table: "Acquisto");

            migrationBuilder.DropForeignKey(
                name: "FK_Acquisto_Proiezioni_ProiezioneId",
                table: "Acquisto");

            migrationBuilder.DropForeignKey(
                name: "FK_Acquisto_Sala_SalaId",
                table: "Acquisto");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Generi_GenereId",
                table: "Movie");

            migrationBuilder.DropForeignKey(
                name: "FK_Proiezioni_Movie_MovieId",
                table: "Proiezioni");

            migrationBuilder.DropForeignKey(
                name: "FK_Proiezioni_Sala_SalaId",
                table: "Proiezioni");

            migrationBuilder.DropForeignKey(
                name: "FK_Sala_FasciaOraria_TurnoId",
                table: "Sala");

            migrationBuilder.DropForeignKey(
                name: "FK_Sala_TipologiaSala_TipologiaSalaId",
                table: "Sala");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipologiaSala",
                table: "TipologiaSala");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sala",
                table: "Sala");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movie",
                table: "Movie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Generi",
                table: "Generi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FasciaOraria",
                table: "FasciaOraria");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Acquisto",
                table: "Acquisto");

            migrationBuilder.RenameTable(
                name: "TipologiaSala",
                newName: "TipologieSala");

            migrationBuilder.RenameTable(
                name: "Sala",
                newName: "Sale");

            migrationBuilder.RenameTable(
                name: "Movie",
                newName: "Movies");

            migrationBuilder.RenameTable(
                name: "Generi",
                newName: "GeneriMovies");

            migrationBuilder.RenameTable(
                name: "FasciaOraria",
                newName: "Turni");

            migrationBuilder.RenameTable(
                name: "Acquisto",
                newName: "Acquisti");

            migrationBuilder.RenameIndex(
                name: "IX_Sala_TurnoId",
                table: "Sale",
                newName: "IX_Sale_TurnoId");

            migrationBuilder.RenameIndex(
                name: "IX_Sala_TipologiaSalaId",
                table: "Sale",
                newName: "IX_Sale_TipologiaSalaId");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_GenereId",
                table: "Movies",
                newName: "IX_Movies_GenereId");

            migrationBuilder.RenameIndex(
                name: "IX_Acquisto_UtenteId",
                table: "Acquisti",
                newName: "IX_Acquisti_UtenteId");

            migrationBuilder.RenameIndex(
                name: "IX_Acquisto_SalaId",
                table: "Acquisti",
                newName: "IX_Acquisti_SalaId");

            migrationBuilder.RenameIndex(
                name: "IX_Acquisto_ProiezioneId",
                table: "Acquisti",
                newName: "IX_Acquisti_ProiezioneId");

            migrationBuilder.RenameIndex(
                name: "IX_Acquisto_MovieId",
                table: "Acquisti",
                newName: "IX_Acquisti_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipologieSala",
                table: "TipologieSala",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sale",
                table: "Sale",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneriMovies",
                table: "GeneriMovies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Turni",
                table: "Turni",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Acquisti",
                table: "Acquisti",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Acquisti_AspNetUsers_UtenteId",
                table: "Acquisti",
                column: "UtenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Acquisti_Movies_MovieId",
                table: "Acquisti",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Acquisti_Proiezioni_ProiezioneId",
                table: "Acquisti",
                column: "ProiezioneId",
                principalTable: "Proiezioni",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Acquisti_Sale_SalaId",
                table: "Acquisti",
                column: "SalaId",
                principalTable: "Sale",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_GeneriMovies_GenereId",
                table: "Movies",
                column: "GenereId",
                principalTable: "GeneriMovies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proiezioni_Movies_MovieId",
                table: "Proiezioni",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proiezioni_Sale_SalaId",
                table: "Proiezioni",
                column: "SalaId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_TipologieSala_TipologiaSalaId",
                table: "Sale",
                column: "TipologiaSalaId",
                principalTable: "TipologieSala",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Turni_TurnoId",
                table: "Sale",
                column: "TurnoId",
                principalTable: "Turni",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acquisti_AspNetUsers_UtenteId",
                table: "Acquisti");

            migrationBuilder.DropForeignKey(
                name: "FK_Acquisti_Movies_MovieId",
                table: "Acquisti");

            migrationBuilder.DropForeignKey(
                name: "FK_Acquisti_Proiezioni_ProiezioneId",
                table: "Acquisti");

            migrationBuilder.DropForeignKey(
                name: "FK_Acquisti_Sale_SalaId",
                table: "Acquisti");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_GeneriMovies_GenereId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Proiezioni_Movies_MovieId",
                table: "Proiezioni");

            migrationBuilder.DropForeignKey(
                name: "FK_Proiezioni_Sale_SalaId",
                table: "Proiezioni");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_TipologieSala_TipologiaSalaId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Turni_TurnoId",
                table: "Sale");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Turni",
                table: "Turni");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipologieSala",
                table: "TipologieSala");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sale",
                table: "Sale");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneriMovies",
                table: "GeneriMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Acquisti",
                table: "Acquisti");

            migrationBuilder.RenameTable(
                name: "Turni",
                newName: "FasciaOraria");

            migrationBuilder.RenameTable(
                name: "TipologieSala",
                newName: "TipologiaSala");

            migrationBuilder.RenameTable(
                name: "Sale",
                newName: "Sala");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "Movie");

            migrationBuilder.RenameTable(
                name: "GeneriMovies",
                newName: "Generi");

            migrationBuilder.RenameTable(
                name: "Acquisti",
                newName: "Acquisto");

            migrationBuilder.RenameIndex(
                name: "IX_Sale_TurnoId",
                table: "Sala",
                newName: "IX_Sala_TurnoId");

            migrationBuilder.RenameIndex(
                name: "IX_Sale_TipologiaSalaId",
                table: "Sala",
                newName: "IX_Sala_TipologiaSalaId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_GenereId",
                table: "Movie",
                newName: "IX_Movie_GenereId");

            migrationBuilder.RenameIndex(
                name: "IX_Acquisti_UtenteId",
                table: "Acquisto",
                newName: "IX_Acquisto_UtenteId");

            migrationBuilder.RenameIndex(
                name: "IX_Acquisti_SalaId",
                table: "Acquisto",
                newName: "IX_Acquisto_SalaId");

            migrationBuilder.RenameIndex(
                name: "IX_Acquisti_ProiezioneId",
                table: "Acquisto",
                newName: "IX_Acquisto_ProiezioneId");

            migrationBuilder.RenameIndex(
                name: "IX_Acquisti_MovieId",
                table: "Acquisto",
                newName: "IX_Acquisto_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FasciaOraria",
                table: "FasciaOraria",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipologiaSala",
                table: "TipologiaSala",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sala",
                table: "Sala",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movie",
                table: "Movie",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Generi",
                table: "Generi",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Acquisto",
                table: "Acquisto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Acquisto_AspNetUsers_UtenteId",
                table: "Acquisto",
                column: "UtenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Acquisto_Movie_MovieId",
                table: "Acquisto",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Acquisto_Proiezioni_ProiezioneId",
                table: "Acquisto",
                column: "ProiezioneId",
                principalTable: "Proiezioni",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Acquisto_Sala_SalaId",
                table: "Acquisto",
                column: "SalaId",
                principalTable: "Sala",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Generi_GenereId",
                table: "Movie",
                column: "GenereId",
                principalTable: "Generi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proiezioni_Movie_MovieId",
                table: "Proiezioni",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proiezioni_Sala_SalaId",
                table: "Proiezioni",
                column: "SalaId",
                principalTable: "Sala",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sala_FasciaOraria_TurnoId",
                table: "Sala",
                column: "TurnoId",
                principalTable: "FasciaOraria",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sala_TipologiaSala_TipologiaSalaId",
                table: "Sala",
                column: "TipologiaSalaId",
                principalTable: "TipologiaSala",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
