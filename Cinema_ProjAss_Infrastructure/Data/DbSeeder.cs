using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinema_ProjAss_Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(CinemaDbContext db)
        {
            // Sørg for at migrations er applied
            await db.Database.MigrateAsync();

            // ============================================================
            // GENRES
            // ============================================================
            await EnsureGenresAsync(db);

            // ============================================================
            // AUDITORIUMS (SALE) + SEATS (AUTO-REPAIR)
            // ============================================================
            await EnsureAuditoriumsAndSeatsAsync(db);

            // ============================================================
            // USERS (Admin + Customers)
            // ============================================================
            await EnsureUsersAsync(db);

            // ============================================================
            // MOVIES (realistiske)
            // ============================================================
            await EnsureMoviesAsync(db);

            // ============================================================
            // SHOWS (forskellige tider/priser/sale)
            // ============================================================
            await EnsureShowsAsync(db);
        }

        // ----------------------------
        // GENRES
        // ----------------------------
        private static async Task EnsureGenresAsync(CinemaDbContext db)
        {
            var genreNames = new[]
            {
                "Action", "Comedy", "Drama", "Sci-Fi", "Animation", "Documentary", "Thriller", "Horror"
            };

            foreach (var name in genreNames)
            {
                if (!await db.Genres.AnyAsync(g => g.Name == name))
                    db.Genres.Add(new Genre { Name = name });
            }

            await db.SaveChangesAsync();
        }

        // ----------------------------
        // AUDITORIUMS + SEATS
        // ----------------------------
        private static async Task EnsureAuditoriumsAndSeatsAsync(CinemaDbContext db)
        {
            // Layout pr sal: (rows, seatsPerRow)
            var layouts = new Dictionary<string, (int rows, int seatsPerRow)>
            {
                ["Sal 1"] = (6, 10),  // A-F, 1-10  => 60
                ["Sal 2"] = (8, 12),  // A-H, 1-12  => 96
                ["Sal 3"] = (5, 8)    // A-E, 1-8   => 40
            };

            // 1) Opret sale hvis de mangler
            foreach (var salName in layouts.Keys)
            {
                if (!await db.Auditoriums.AnyAsync(a => a.Name == salName))
                    db.Auditoriums.Add(new Auditorium { Name = salName });
            }
            await db.SaveChangesAsync();

            // 2) Auto-genskab Seats pr. sal hvis de mangler / er forkert antal
            var auditoriums = await db.Auditoriums.ToListAsync();

            foreach (var a in auditoriums)
            {
                if (!layouts.TryGetValue(a.Name, out var layout))
                    continue;

                var expected = layout.rows * layout.seatsPerRow;
                var current = await db.Seats.CountAsync(s => s.AuditoriumId == a.Id);

                // Hvis du har slettet Seats (eller count ikke matcher), så rebuild seats for salen
                if (current != expected)
                {
                    // Slet kun sæder for den sal (ikke resten)
                    var existing = db.Seats.Where(s => s.AuditoriumId == a.Id);
                    db.Seats.RemoveRange(existing);
                    await db.SaveChangesAsync();

                    CreateSeats(db, a.Id, layout.rows, layout.seatsPerRow);
                    await db.SaveChangesAsync();
                }
            }
        }

        private static void CreateSeats(CinemaDbContext db, int auditoriumId, int rows, int seatsPerRow)
        {
            for (char row = 'A'; row < 'A' + rows; row++)
            {
                for (int number = 1; number <= seatsPerRow; number++)
                {
                    db.Seats.Add(new Seat
                    {
                        AuditoriumId = auditoriumId,
                        Row = row.ToString(),
                        Number = number
                    });
                }
            }
        }

        // ----------------------------
        // USERS
        // ----------------------------
        private static async Task EnsureUsersAsync(CinemaDbContext db)
        {
            // Vi laver "upsert" pr username, så du kan re-run uden duplicates (Username er unik)
            await UpsertUserAsync(db, username: "admin", password: "Admin123!", role: "Admin");
            await UpsertUserAsync(db, username: "bob", password: "Bob123!", role: "Customer");
            await UpsertUserAsync(db, username: "alice", password: "Alice123!", role: "Customer");
        }

        private static async Task UpsertUserAsync(CinemaDbContext db, string username, string password, string role)
        {
            var existing = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existing != null)
            {
                // Hvis du vil kunne opdatere rolle på seed:
                if (existing.Role != role)
                {
                    existing.Role = role;
                    await db.SaveChangesAsync();
                }
                return;
            }

            var hasher = new PasswordHasher<AppUser>();

            var user = new AppUser
            {
                Username = username,
                Role = role,
                PasswordSalt = Array.Empty<byte>() // du bruger PasswordHasher, så denne er "ikke i brug" lige nu
            };

            user.PasswordHash = hasher.HashPassword(user, password);

            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        // ----------------------------
        // MOVIES
        // ----------------------------
        private static async Task EnsureMoviesAsync(CinemaDbContext db)
        {
            // Genre lookup
            var action = await db.Genres.FirstAsync(g => g.Name == "Action");
            var comedy = await db.Genres.FirstAsync(g => g.Name == "Comedy");
            var drama = await db.Genres.FirstAsync(g => g.Name == "Drama");
            var scifi = await db.Genres.FirstAsync(g => g.Name == "Sci-Fi");
            var animation = await db.Genres.FirstAsync(g => g.Name == "Animation");
            var thriller = await db.Genres.FirstAsync(g => g.Name == "Thriller");
            var documentary = await db.Genres.FirstAsync(g => g.Name == "Documentary");
            var horror = await db.Genres.FirstAsync(g => g.Name == "Horror");

            // Helper til upsert movie pr title
            async Task UpsertMovie(string title, int durationMinutes, params int[] genreIds)
            {
                var exists = await db.Movies.AnyAsync(m => m.Title == title);
                if (exists) return;

                var movie = new Movie
                {
                    Title = title,
                    DurationMinutes = durationMinutes,
                    MovieGenres = genreIds.Select(id => new MovieGenre { GenreId = id }).ToList()
                };

                db.Movies.Add(movie);
                await db.SaveChangesAsync();
            }

            await UpsertMovie("Interstellar", 169, scifi.Id, drama.Id);
            await UpsertMovie("The Dark Knight", 152, action.Id, thriller.Id);
            await UpsertMovie("Finding Nemo", 100, animation.Id, comedy.Id);
            await UpsertMovie("Joker", 122, drama.Id, thriller.Id);
            await UpsertMovie("The Conjuring", 112, horror.Id, thriller.Id);
            await UpsertMovie("Planet Earth: The Movie", 95, documentary.Id);
            await UpsertMovie("Rush Hour", 98, action.Id, comedy.Id);
        }

        // ----------------------------
        // SHOWS
        // ----------------------------
        private static async Task EnsureShowsAsync(CinemaDbContext db)
        {
            // Hvis du vil reseed shows hver gang, kan man slette shows her,
            // men vi gør det sikkert: vi tilføjer kun hvis Shows-tabellen er tom.
            if (await db.Shows.AnyAsync())
                return;

            var sal1 = await db.Auditoriums.FirstAsync(a => a.Name == "Sal 1");
            var sal2 = await db.Auditoriums.FirstAsync(a => a.Name == "Sal 2");
            var sal3 = await db.Auditoriums.FirstAsync(a => a.Name == "Sal 3");

            var interstellar = await db.Movies.FirstAsync(m => m.Title == "Interstellar");
            var darkKnight = await db.Movies.FirstAsync(m => m.Title == "The Dark Knight");
            var nemo = await db.Movies.FirstAsync(m => m.Title == "Finding Nemo");
            var joker = await db.Movies.FirstAsync(m => m.Title == "Joker");
            var conjuring = await db.Movies.FirstAsync(m => m.Title == "The Conjuring");
            var planetEarth = await db.Movies.FirstAsync(m => m.Title == "Planet Earth: The Movie");
            var rushHour = await db.Movies.FirstAsync(m => m.Title == "Rush Hour");

            // Start fra i morgen (så du altid ser “kommende” shows)
            var day = DateTime.Today.AddDays(1);

            var shows = new List<Show>
            {
                // Dag 1
                new Show { MovieId = nemo.Id,        AuditoriumId = sal3.Id, StartTime = day.AddHours(14), Price = 99m },
                new Show { MovieId = interstellar.Id,AuditoriumId = sal1.Id, StartTime = day.AddHours(18), Price = 139m },
                new Show { MovieId = darkKnight.Id,  AuditoriumId = sal2.Id, StartTime = day.AddHours(20), Price = 159m },

                // Dag 2
                new Show { MovieId = planetEarth.Id, AuditoriumId = sal1.Id, StartTime = day.AddDays(1).AddHours(11), Price = 89m },
                new Show { MovieId = rushHour.Id,    AuditoriumId = sal3.Id, StartTime = day.AddDays(1).AddHours(17), Price = 119m },
                new Show { MovieId = joker.Id,       AuditoriumId = sal2.Id, StartTime = day.AddDays(1).AddHours(21), Price = 149m },

                // Dag 3
                new Show { MovieId = nemo.Id,        AuditoriumId = sal3.Id, StartTime = day.AddDays(2).AddHours(13), Price = 109m },
                new Show { MovieId = interstellar.Id,AuditoriumId = sal2.Id, StartTime = day.AddDays(2).AddHours(19), Price = 169m },
                new Show { MovieId = conjuring.Id,   AuditoriumId = sal1.Id, StartTime = day.AddDays(2).AddHours(22), Price = 149m },
            };

            db.Shows.AddRange(shows);
            await db.SaveChangesAsync();
        }
    }
}
