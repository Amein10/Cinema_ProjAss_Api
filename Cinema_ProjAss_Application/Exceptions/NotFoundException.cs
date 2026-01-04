using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.Exceptions
{
    /// <summary>
    /// Bruges når en ressource ikke findes (404 i API-laget).
    /// </summary>
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
