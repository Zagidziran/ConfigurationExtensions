using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zagidziran.ConfigurationExtensions.Substitutions
{
    internal static class SubstitutionLocator
    {
        // We want to handle the @(someRef) as mappings and ${C# code} as code
        private static readonly Regex substitutionsRegex = new Regex(@"(\$\{(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))\})|(\$\(.*?\))", RegexOptions.Compiled);

        public static IEnumerable<Substitution> FindSubstitutions(string text)
        {
            return substitutionsRegex
                .Matches(text)
                .Select(FromRegexMatch);
        }

        private static Substitution FromRegexMatch(Match match)
        {
            var kind = match switch
            {
                _ when match.Value.StartsWith("$(") => SubstitutionKind.Mapping,
                _ when match.Value.StartsWith("${") => SubstitutionKind.Code,
                _ => throw new ApplicationException("It should not ever happen!"),
            };

            return new Substitution(
                kind,
                match.Index,
                match.Length,
                match.Value.Substring(2, match.Length - 3).Trim(),
                match.Value);
        }
    }
}
