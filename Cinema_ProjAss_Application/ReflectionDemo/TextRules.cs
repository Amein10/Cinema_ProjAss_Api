using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.ReflectionDemo
{
    /// <summary>
    /// Demo-klasse til Reflection opgaven.
    /// Har metoden med signaturen: public bool StortBogstav(string stort)
    /// </summary>
    public class TextRules
    {
        // KRAVET fra opgaven:
        public bool StortBogstav(string stort)
        {
            if (string.IsNullOrWhiteSpace(stort)) return false;

            // returnerer true hvis første tegn er stort bogstav
            return char.IsUpper(stort.Trim()[0]);
        }

        // (Valgfrit) Flere metoder med samme signatur kan også findes af reflection:
        public bool StortBogstav2(string stort)
        {
            if (string.IsNullOrWhiteSpace(stort)) return false;
            return stort.Trim().All(c => !char.IsLetter(c) || char.IsUpper(c));
        }
    }
}
