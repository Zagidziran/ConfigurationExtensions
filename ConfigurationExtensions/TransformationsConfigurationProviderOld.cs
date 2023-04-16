//namespace Zagidziran.ConfigurationExtensions
//{
//    using System.Collections.Generic;
//    using System.Diagnostics.CodeAnalysis;
//    using System.Linq;
//    using System.Text;
//    using System.Text.RegularExpressions;
//    using Microsoft.Extensions.Configuration;
//    using Zagidziran.ConfigurationExtensions.Exceptions;

//    internal sealed class TransformationsConfigurationProvider : ConfigurationProvider
//    {
//        private readonly Dictionary<string, string> originalConfiguration;

//        private readonly Regex singleStatementRegex = new Regex(@"(\$\(.*?\))", RegexOptions.Compiled);

//        public TransformationsConfigurationProvider(
//            IEnumerable<KeyValuePair<string, string>> originalConfiguration)
//        {
//            this.originalConfiguration = new Dictionary<string, string>(originalConfiguration);
//        }

//        public override void Load()
//        {
//            foreach (var configurationItem in this.originalConfiguration)
//            {
//                if (configurationItem.Value == null)
//                {
//                    continue;
//                }

//                var matches = this.singleStatementRegex.Matches(configurationItem.Value);

//                // If it is the full match we can copy full hierarcy
//                if (matches.Count == 1 && matches[0].Value == configurationItem.Value)
//                {
//                    var prefix = this.ExtractKeyFromMatch(matches[0]);
//                    var keys = this.originalConfiguration
//                        .Where(k => k.Key.StartsWith(prefix + ":") || k.Key == prefix)
//                        .ToList();

//                    if (!keys.Any())
//                    {
//                        throw new ReferencedKeyNotFoundExcepion(configurationItem.Key, this.ExtractKeyFromMatch(matches[0]), "Referenced key not found.");
//                    }

//                    foreach (var configurationKey in keys)
//                    {
//                        var resultingKey = configurationItem.Key + configurationKey.Key.Substring(prefix.Length);
//                        this.Data[resultingKey] = configurationKey.Value;
//                    }
//                }
//                else if(matches.Count > 0)
//                {
//                    var stringBuilder = new StringBuilder();
//                    var currentPosition = 0;
//                    foreach (Match match in matches)
//                    {
//                        if (!this.TryEvaluateMatch(match, out var replacement))
//                        {
//                            throw new ReferencedKeyNotFoundExcepion(configurationItem.Key, this.ExtractKeyFromMatch(match), "Referenced key not found.");
//                        }

//                        stringBuilder.Append(configurationItem.Value.Substring(currentPosition, match.Index));
//                        stringBuilder.Append(replacement);
//                        currentPosition = match.Index + match.Length;
//                    }

//                    this.Data[configurationItem.Key] = stringBuilder.ToString();
//                }
//            }
//        }

//        private bool TryEvaluateMatch(Match match, [NotNullWhen(true)]out string? value)
//        {
//            var dictionaryKey = this.ExtractKeyFromMatch(match);
            
//            if (this.originalConfiguration.ContainsKey(dictionaryKey))
//            { 
//                value = this.originalConfiguration[dictionaryKey];
//                return true;
//            }

//            value = null;
//            return false;
//        }

//        private string ExtractKeyFromMatch(Match match)
//        {
//            // Extracting value from @(operator)
//            return match.Value.Substring(2, match.Length - 3).Replace(".", ":");
//        }
//    }
//}