using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Cinema_ProjAss_Application.ReflectionDemo
{
    public record ParsedMethodResult(
        string DeclaringType,
        string MethodName,
        bool? Result,
        string Status
    );

    /// <summary>
    /// Finder og eksekverer metoder via Reflection med signaturen:
    /// public bool X(string)
    /// Her matcher vi specifikt navnet "StortBogstav" (og kan nemt udvides).
    /// </summary>
    public static class ReflectionMethodRunner
    {
        public static IReadOnlyList<ParsedMethodResult> RunStortBogstavMethods(string input)
        {
            var assembly = typeof(TextRules).Assembly; // vi scanner Application assembly (hvor TextRules ligger)

            var results = new List<ParsedMethodResult>();

            foreach (var type in assembly.GetTypes())
            {
                // Kun almindelige klasser (ikke abstract/interface)
                if (!type.IsClass || type.IsAbstract) continue;

                // Find public instance metoder med navn "StortBogstav" og signaturen bool(string)
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m =>
                        m.Name == "StortBogstav" &&
                        m.ReturnType == typeof(bool) &&
                        m.GetParameters().Length == 1 &&
                        m.GetParameters()[0].ParameterType == typeof(string)
                    )
                    .ToList();

                if (methods.Count == 0) continue;

                // Prøv at oprette instans (kræver parameterløs constructor)
                object? instance = null;
                try
                {
                    instance = Activator.CreateInstance(type);
                }
                catch
                {
                    foreach (var m in methods)
                    {
                        results.Add(new ParsedMethodResult(
                            type.FullName ?? type.Name,
                            m.Name,
                            null,
                            "Skipped: no parameterless constructor"
                        ));
                    }
                    continue;
                }

                foreach (var m in methods)
                {
                    try
                    {
                        var value = m.Invoke(instance, new object[] { input });
                        results.Add(new ParsedMethodResult(
                            type.FullName ?? type.Name,
                            m.Name,
                            (bool)value!,
                            "OK"
                        ));
                    }
                    catch (Exception ex)
                    {
                        results.Add(new ParsedMethodResult(
                            type.FullName ?? type.Name,
                            m.Name,
                            null,
                            $"Error: {ex.GetBaseException().Message}"
                        ));
                    }
                }
            }

            // Hvis ingen metoder blev fundet, så returner en forklarende entry
            if (results.Count == 0)
            {
                results.Add(new ParsedMethodResult(
                    "N/A",
                    "StortBogstav",
                    null,
                    "No matching methods found (public bool StortBogstav(string))"
                ));
            }

            return results;
        }
    }
}

