using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    public static class IpaHelpers
    {
        public static string[] ParseSymbols(string input)
        {
            IEnumerable<string> Helper()
            {
                for (var en = StringInfo.GetTextElementEnumerator(input); en.MoveNext();)
                    yield return en.GetTextElement();
            }
            return Helper().ToArray();
        }

        private static char[] _vowelSymbols = new[] { 'i', 'e', 'ɛ', 'a', 'ɑ', 'ɔ', 'o', 'u', 'y', 'ø', 'œ', 'ə', };
        public static bool IsVowel(string input)
        {
            return _vowelSymbols.Contains(input[0]);
        }
    }
}
