using Intervals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bard.Fra.Analysis
{
    public class PhonologicalAligner2
    {
        private string _written;
        private string[] _phonemes;

        public PhonologicalAligner2(string written, string[] phonemes)
        {
            _written = written;
            _phonemes = phonemes;
        }

        public Interval<string>[] Compute()
        {
            var alignments = new Queue<Interval<string>>();
            if (TryAlignNext(0, 0, ref alignments))
                return alignments.ToArray();
            else
                return null;
        }

        private bool TryAlignNext(int phonemeIdx, int graphemeIdx, ref Queue<Interval<string>> alignment)
        {
            if (phonemeIdx == _phonemes.Length)
            {
                if (graphemeIdx < _written.Length)
                    alignment.Enqueue(new Interval<string>(graphemeIdx, _written.Length - graphemeIdx, string.Empty));

                return true;
            }

            var phoneme = _phonemes[phonemeIdx];
            var patterns = GetWrittenForms(phoneme);

            foreach (var pattern in patterns)
            {
                int length = pattern.Length;
                if (_written.Substring(graphemeIdx, length) == pattern)
                {
                    alignment.Enqueue(new Interval<string>(graphemeIdx, length, phoneme));

                    if (TryAlignNext(phonemeIdx + 1, graphemeIdx + length, ref alignment))
                        return true;
                    else
                        alignment.Dequeue();
                }
            }

            return false;
        }

        private string[] GetWrittenForms(string phoneme)
        {
            if (_writtenForms.TryGetValue(phoneme, out var forms))
                return forms;
            else
                throw new Exception($"Unsupported phoneme [{phoneme}].");
        }

        private Dictionary<string, string[]> _writtenForms = new Dictionary<string, string[]>()
        {
            { "p", new string[] { "pph", "pp", "ph", "p" } },
            { "t", new string[] { "tth", "tt", "t" }},
            { "k", new string[] { "cch", "cc", "ch", "kkh", "kk", "kh", "qu", "c", "k", "q" }},
            { "b", new string[] { "bbh", "bb", "bh", "b" }},
            { "d", new string[] { "ddh", "dd", "dh", "d" }},
            { "g", new string[] { "ggh", "gu", "gg", "gh", "g" }},
            { "f", new string[] { "ff", "f", "ph" }},
            { "s", new string[] { "ss", "s", "x" }},
            { "ʃ", new string[] { "ch", "sh" }},
            { "v", new string[] { "vv", "v", "w" }},
            { "z", new string[] { "zz", "s", "z", }}, // x?
            { "ʒ", new string[] { "j", "g" }},
            { "l", new string[] { "ll", "l" }},
            { "ʁ", new string[] { "rrh", "rr", "rh", "r" }},
            { "m", new string[] { "mm", "m" }},
            { "n", new string[] { "nn", "mn", "n" }},
            { "ɲ", new string[] { "gn" }},
            { "ŋ", new string[] { "ng" }},
            { "x", new string[] { "j" }},

            { "j", new string[] { "ill", "il", "y", "j", "ï" }},
            { "w", new string[] { "hou", "ou", "ww", "wh", "w" }},
            { "ɥ", new string[] { "hu", "u" }},

            { "i", new string[] { "hi", "hy", "hï", "hî", "i", "y", "ï", "î" }},
            { "e", new string[] { "hé", "he", "ez", "é", "e" }},
            { "ɛ", new string[] { "hai", "hè", "hê", "hë", "ai", "è", "ê", "ë" }},
            { "a", new string[] { "has", "ha", "hâ", "as", "a", "â" }},
            { "ɑ", new string[] { "has", "ha", "hâ", "as", "a", "â" }},
            { "ɔ", new string[] { "ho", "o" }},
            { "o", new string[] { "hau", "au", "ho", "hô", "o", "ô" }},
            { "u", new string[] { "hou", "ou", "où", "oû", "hu", "u" }},
            { "y", new string[] { "hu", "u" }},
            { "ø", new string[] { "heu", "eu" }},
            { "œ", new string[] { "heu", "eu" }},
            { "ə", new string[] { "he", "e" }},
            { "ɛ̃", new string [] { "hain", "hein", "hin", "hen", "ain", "ein", "in", "en" } },
            { "ɑ̃", new string [] { "han", "ham", "hen", "hem", "an", "am", "en", "em" }},
            { "ɔ̃", new string [] { "hon", "hom", "on", "om" }},
            { "œ̃", new string [] { "hun", "hum", "un", "um" }},
        };
    }
}
