using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology.Tests
{
    public abstract class TestBase
    {
        protected string[] ParsePhonemes(string phonemesRaw)
        {
            var builder = new List<string>();

            foreach (var c in phonemesRaw)
            {
                if (_phonemeByChar.TryGetValue(c, out var phoneme))
                    builder.Add(phoneme);
                else
                    builder.Add(c.ToString());
            }

            return builder.ToArray();
        }

        protected static Dictionary<char, string> _phonemeByChar = new Dictionary<char, string>()
        {
            { 'O', "ɔ" }, { 'E', "ɛ" }, { '°', "ə" }, { '2', "ø" }, { '9', "œ" },
            { '5', "ɛ̃" }, { '1', "œ̃" }, { '@', "ɑ̃" }, { '§', "ɔ̃" }, { '8', "ɥ" },
            { 'S', "ʃ" }, { 'Z', "ʒ" }, { 'N', "ɲ" }, { 'R', "ʁ" }, { 'x', "χ" },
            { 'G', "ŋ" }, { 'A', "ɑ" }
        };
    }
}
