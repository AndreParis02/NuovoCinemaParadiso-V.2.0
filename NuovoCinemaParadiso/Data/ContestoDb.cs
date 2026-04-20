using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Data
{
    public class ContestoDb : IdentityDbContext<Utente, IdentityRole, string>
    {
        public ContestoDb(DbContextOptions<ContestoDb> opzioni)
            : base(opzioni)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<GenereMovie> GeneriMovies { get; set; }
        public DbSet<Sala> Sale { get; set; }
        public DbSet<TipologiaSala> TipologieSala { get; set; }
        public DbSet<Turno> Turni { get; set; }
        public DbSet<Acquisto> Acquisti { get; set; }
        public DbSet<Utente> Utenti { get; set; }
        public DbSet<Abbonamento> Abbonamenti {get;set;}
        public DbSet<LogAzioni> LogAzioni {get;set;}
        public DbSet<Proiezione> Proiezioni {get;set;}
        public DbSet<GiftCard> GiftCards {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Supponendo che la tua classe si chiami 'Acquisto' o 'Scontrino'
            // e che abbia una proprietà 'ProiezioneId'
            modelBuilder.Entity<Acquisto>()
                .HasOne(a => a.Proiezione)
                .WithMany(p => p.Acquisti) // o il nome della collezione in Proiezione
                .HasForeignKey(a => a.ProiezioneId)
                .OnDelete(DeleteBehavior.Restrict); 
                // 'Restrict' impedisce la cancellazione della proiezione se esistono scontrini
        }
    }
}