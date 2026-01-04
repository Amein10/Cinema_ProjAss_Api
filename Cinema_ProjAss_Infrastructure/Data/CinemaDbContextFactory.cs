using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cinema_ProjAss_Infrastructure.Data
{
    /// <summary>
    /// Design-time factory til EF Core migrations.
    /// Bruges af "dotnet ef" når den skal kunne oprette en DbContext uden at køre API-projektet.
    /// </summary>
    public class CinemaDbContextFactory : IDesignTimeDbContextFactory<CinemaDbContext>
    {
        /// <summary>
        /// Opretter en DbContext til design-time brug (migrations).
        /// </summary>
        /// <param name="args">Kommandolinje-argumenter (ikke nødvendigvis brugt).</param>
        /// <returns>En konfigureret CinemaDbContext.</returns>
        public CinemaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CinemaDbContext>();

            // Connection string til LocalDB (samme idé som i appsettings).
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=CinemaProjAssDb;Trusted_Connection=True;TrustServerCertificate=True");

            return new CinemaDbContext(optionsBuilder.Options);
        }
    }
}
