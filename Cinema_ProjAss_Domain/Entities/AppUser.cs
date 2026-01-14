/// <summary>
/// 0.1 Domæne Lag:
/// 1. Definerer hvad systemet består af (Movie, Booking, Show…)
/// 2. Definerer “kontrakten” for data-adgang via repository interfaces
/// 3. Indeholder ingen database-kode, ingen EF, ingen HTTP
/// 
/// 0.2 Filens overordnede formål:
/// AppUser er en domain entity, som repræsenterer en bruger i systemet. Den indeholder kun data, ingen forretningslogik,
/// og bruges på tværs af lag til autentificering og autorisation via JWT.
/// 
/// 0.3 Generalt:
/// Jeg bruger using-statements til at fortælle C#-kompileren, hvilke namespaces der skal være tilgængelige i denne fil.
/// Et namespace indeholder klasser og typer, som kan bruges i koden uden at skrive deres fulde navn.
/// 
/// 1. using System:
/// Dette namespace giver adgang til grundlæggende .NET-typer som string, int, DateTime og Array.
/// I denne klasse bruges System bl.a. til DateTime (CreatedAt) og Array.Empty<byte>() (PasswordSalt).
/// 
/// 2. using System.Collections.Generic:
/// Indeholder generiske samlinger(Collections) som List<T> og Dictionary<TKey, TValue>.
/// I denne AppUser-klasse anvendes der ingen samlinger, så dette namespace er ikke nødvendigt her,
/// men det bruges i andre entities i projektet (fx Booking med flere Seats).
/// 
/// 3. using System.Linq:
/// Giver adgang til LINQ-funktionalitet, som gør det muligt at udføre forespørgsler på samlinger,
/// f.eks. Where(), Select() og Any().
/// LINQ anvendes ikke direkte i denne klasse, da AppUser er en ren data-entity uden logik.
/// 
/// 4. using System.Text:
/// Indeholder klasser til teksthåndtering, såsom StringBuilder.
/// Dette namespace anvendes ikke i AppUser-klassen, da der ikke udføres avanceret strengmanipulation.
///
/// 5. using System.Threading.Tasks:
/// Indeholder typer til asynkrone operationer som Task og async/await.
/// AppUser indeholder ingen asynkrone metoder og benytter derfor ikke dette namespace.
///
/// 6. namespace Cinema_ProjAss_Domain.Entities:
/// Dette namespace indeholder domæne-entiteter for biografapplikationen.
/// AppUser er en del af domænelaget og repræsenterer en bruger i systemets kerne-model.
/// 
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// 1. public class AppUser
    /// 
    /// 1.1 Klassen AppUser er deklareret med access modifieren "public",
    /// hvilket betyder at den kan anvendes på tværs af projekter og lag i den lagdelte arkitektur.
    ///
    /// 1.2 En class i C# er en reference type, hvilket betyder at objekter
    /// af denne type håndteres via referencer i hukommelsen.
    ///
    /// 1.3 Navnet "AppUser" indikerer, at klassen repræsenterer en applikationsbruger i systemet.
    /// 
    /// </summary>
    public class AppUser
    {
        /// <summary>
        /// 2. public int Id
        ///
        /// 2.0 
        /// Denne property repræsenterer brugerens Id.
        /// Id bruges som primærnøgle i databasen til entydigt at identificere en AppUser.
        /// 
        /// 2.1
        /// Propertyen er public, så den kan tilgås fra andre klasser og lag i applikationen.
        /// 
        /// 2.2
        /// Datatypen er int, som er velegnet til primærnøgler i databaser.
        /// 
        /// 2.3
        /// Id behandles som primærnøgle af Entity Framework Core på grund af navngivningskonventionen "Id".
        /// Værdien genereres automatisk af databasen.
        /// 
        /// 2.4
        /// Propertyen har både en getter og en setter, hvilket gør det muligt at læse og tildele værdien via objektet.
        /// En property er en variabel i en klasse, som man kan læse og/eller ændre udefra ved hjælp af get og set.
        /// Den bruges til at gemme data om et objekt, fx et brugernavn eller en rolle.
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 3. public string Username
        ///
        /// 3.1
        /// Denne property repræsenterer brugerens brugernavn.
        /// Brugernavnet anvendes til login og identifikation af brugeren i systemet.
        /// 
        /// 3.2.
        /// Propertyen er public, så den kan tilgås fra andre klasser og lag,
        /// f.eks. i Application-laget ved login eller i API-laget ved oprettelse af JWT.
        ///
        /// 3.3
        /// Datatypen er string, som bruges til tekstdata såsom brugernavne.
        /// String er en reference type i C#.
        ///
        /// 3.4
        /// Propertyen initialiseres med string.Empty for at undgå null-værdier
        /// og for at gøre objektet mere robust.
        ///
        /// 3.5
        /// Brugernavnet skal være unikt i databasen, hvilket håndteres via
        /// en unik constraint/index i databasen og ikke direkte i denne klasse.
        /// 
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// 4. public string PasswordHash
        ///
        /// 4.1
        /// Denne property indeholder det hashede password for brugeren.
        /// Det oprindelige password gemmes aldrig i klartekst af sikkerhedsmæssige årsager.
        ///
        /// 4.2
        /// Propertyen er public, så den kan tilgås fra andre klasser og lag,
        /// f.eks. i Application-laget, hvor passwordet valideres ved login.
        ///
        /// 4.3
        /// Datatypen er string, da et hash typisk gemmes som en tekststreng
        /// (f.eks. Base64- eller hexadecimal-repræsentation).
        ///
        /// 4.4
        /// Hash-værdien genereres i Application-laget ved hjælp af kryptografiske
        /// hash-funktioner og gemmes herefter i databasen.
        /// 
        /// 4.5
        /// Propertyens hash-værdi kan blive læst og ændret af andre klasser grundet dens get og set.
        /// 
        /// 4.6 
        /// Propertyen initialiseres med string.Empty for at undgå null-værdier
        /// og for at gøre objektet mere robust.
        ///
        /// 4.7
        /// Domain-klassen indeholder ingen logik til hashing, da ansvaret
        /// for sikkerhed og validering ligger i Application-laget.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = "Customer";
    }
}
