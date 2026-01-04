using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema_ProjAss_Infrastructure.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for biograf-systemet.
    /// DbContext'en definerer DbSets (tabeller) og konfigurerer relationer/constraints
    /// mellem Domain entities via Fluent API.
    /// </summary>
    public class CinemaDbContext : DbContext
    {
        /// <summary>
        /// Opretter en ny DbContext med de givne options (connection string, provider osv.).
        /// </summary>
        /// <param name="options">Konfiguration for DbContext (fx SQL Server).</param>
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
            : base(options)
        {
        }

        // -------------------------
        // DbSets (tabeller)
        // -------------------------

        /// <summary>Film (Movie).</summary>
        public DbSet<Movie> Movies => Set<Movie>();

        /// <summary>Genrer (Genre).</summary>
        public DbSet<Genre> Genres => Set<Genre>();

        /// <summary>Mange-til-mange kobling mellem Movie og Genre.</summary>
        public DbSet<MovieGenre> MovieGenres => Set<MovieGenre>();

        /// <summary>Visninger/forestillinger (Show).</summary>
        public DbSet<Show> Shows => Set<Show>();

        /// <summary>Biografsale (Auditorium).</summary>
        public DbSet<Auditorium> Auditoriums => Set<Auditorium>();

        /// <summary>Sæder (Seat).</summary>
        public DbSet<Seat> Seats => Set<Seat>();

        /// <summary>Bookinger (Booking).</summary>
        public DbSet<Booking> Bookings => Set<Booking>();

        /// <summary>Kobling mellem Booking og Seat (mange-til-mange).</summary>
        public DbSet<BookingSeat> BookingSeats => Set<BookingSeat>();

        /// <summary>Betalinger (Payment).</summary>
        public DbSet<Payment> Payments => Set<Payment>();

        /// <summary>
        /// Konfigurerer relationer, primærnøgler, constraints, og datatyper.
        /// Kører når EF bygger model-metadata.
        /// </summary>
        /// 
        public DbSet<AppUser> Users => Set<AppUser>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------------
            // MovieGenre (M:M) - composite key
            // -------------------------
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId);

            // -------------------------
            // BookingSeat (M:M) - composite key
            // -------------------------
            modelBuilder.Entity<BookingSeat>()
                .HasKey(bs => new { bs.BookingId, bs.SeatId });

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId);

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Seat)
                .WithMany(s => s.BookingSeats)
                .HasForeignKey(bs => bs.SeatId)
                // Restrict: hvis et sæde er referenced af bookings, skal man ikke kunne slette sædet uden at håndtere historik
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Show relations
            // -------------------------
            modelBuilder.Entity<Show>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Shows)
                .HasForeignKey(s => s.MovieId);

            modelBuilder.Entity<Show>()
                .HasOne(s => s.Auditorium)
                .WithMany(a => a.Shows)
                .HasForeignKey(s => s.AuditoriumId);

            // -------------------------
            // Seat -> Auditorium (1:M)
            // -------------------------
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Auditorium)
                .WithMany(a => a.Seats)
                .HasForeignKey(s => s.AuditoriumId);

            // Unik constraint: (AuditoriumId, Row, Number) bør være unik, så man ikke kan oprette samme sæde to gange.
            modelBuilder.Entity<Seat>()
                .HasIndex(s => new { s.AuditoriumId, s.Row, s.Number })
                .IsUnique();

            // -------------------------
            // Booking -> Show (1:M)
            // -------------------------
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Show)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ShowId);

            // -------------------------
            // Payment -> Booking (1:1-ish)
            // -------------------------
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId);

            // -------------------------
            // Decimal precision (vigtigt til SQL Server)
            // -------------------------
            modelBuilder.Entity<Show>()
                .Property(s => s.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BookingSeat>()
                .Property(bs => bs.PriceAtBooking)
                .HasPrecision(10, 2);

            // Genre.Name bør være unik, så vi ikke får "Action" flere gange.
            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.Name)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Username)
                .IsUnique();

        }
    }
}
