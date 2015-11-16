﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rot13
{
    public static class Extensions
    {
        private static Dictionary<char, char> subs;

        static void Init() // Called when the dictionary is needed
        {
            var alpha = "abcdefghijklmnopqrstuvwxyz";
            subs = MakePairs(alpha).ToDictionary(k => k.Key, k=>k.Value);
        }

        static IEnumerable<KeyValuePair<char, char>> MakePairs(string alpha)
        {
            for (int i = 0; i < alpha.Length; i++)
            {
                var s1 = alpha[i];
                var r1 = alpha[(i + 13) % 26];
                yield return new KeyValuePair<char, char>(s1, r1);
                var s2 = alpha.ToUpperInvariant()[i];
                var r2 = alpha.ToUpperInvariant()[(i + 13) % 26];
                yield return new KeyValuePair<char, char>(s2, r2);
            }
        }

        public static string Rot13(this string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (subs == null) Init();

            var sb = new StringBuilder(value.Length);
            sb.Append(
                value.ToCharArray().Select(c =>
                {
                    char c1;
                    return subs.TryGetValue(c, out c1) ? c1 : c;
                })
                .ToArray() // would be nicer to have Stringbuilder.Append(IEnumerable<char>)
                           // as an extension method but Append(object) already exists so 
                           // extension methods are never called :-(
            );
            return sb.ToString();
        }
    }
}