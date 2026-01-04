using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.Exceptions
{
    /// <summary>
    /// Bruges til input-valideringsfejl (400 i API-laget).
    /// </summary>
    public sealed class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}

