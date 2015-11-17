using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rot13
{
    public static class Extensions
    {
        private static Dictionary<char, char> substitutions;

        /// <summary>
        /// Performs ROT13 encoding on a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>An encodes string</returns>
        public static string Rot13(this string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (substitutions == null) Rot13Init();

            var sb = new StringBuilder(value.Length);
            sb.Append(
                value.ToCharArray().Select(c =>
                {
                    char c1;
                    return substitutions.TryGetValue(c, out c1) ? c1 : c;
                })
                .ToArray() // would be nicer to have Stringbuilder.Append(IEnumerable<char>)
                           // as an extension method but Append(object) already exists so 
                           // extension methods are never called :-(
            );
            return sb.ToString();
        }

        /// <summary>
        /// Lazy init of substitution dictionary to avoid using static constructor
        /// </summary>
        private static void Rot13Init()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            substitutions = Rot13MakePairs(alphabet).Concat(
                            Rot13MakePairs(alphabet.ToUpperInvariant())
                            ).ToDictionary(k => k.Key, k => k.Value);
        }

        /// <summary>
        /// Turns a (mostly) arbitrary string into a set of key-value pairs suitable for rotational encoding
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns k/v pairs of rotated data, eg (m,z)</returns>
        private static IEnumerable<KeyValuePair<char, char>> Rot13MakePairs(string value)
        {
            // make sure value really exists
            if (value == null) throw new ArgumentNullException(nameof(value));
            var chars = value.ToCharArray();
            var length = chars.Length;

            // validate input
            if (length % 2 != 0) throw new ArgumentException("String must have even number of characters");
            if (chars.Distinct().Count() != length) throw new ArgumentException("Characters in string must be unique");

            var shift = length / 2;
            // i is a counter which is incremented for each item in the sequence
            return chars.Select((c, i) => new KeyValuePair<char, char>(c, chars[(shift + i) % length]));
        }
    }
}