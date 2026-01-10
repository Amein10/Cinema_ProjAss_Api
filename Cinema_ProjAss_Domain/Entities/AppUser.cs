using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Simpel bruger-entity til JWT login (uden Identity).
    /// </summary>
    public class AppUser
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Password hash (aldrig gem plain text password).
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Brug string for simplicity: "Customer" / "Admin"
        public string Role { get; set; } = "Customer";
    }
}
