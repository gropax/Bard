using Intervals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bard.Fra.Analysis
{
    public class NaivePhonologicalAligner
    {
        private string _graphemes;
        private string _lowerCased;
        private string[] _phonemes;
        private StringBuilder _trace;

        public NaivePhonologicalAligner(string graphemes, string[] phonemes)
        {
            _graphemes = graphemes;
            _lowerCased = graphemes.ToLower();
            _phonemes = phonemes;
        }

        public string GetTrace() => _trace.ToString();

        public Interval<string>[] Compute()
        {
            _trace = new StringBuilder();
            _trace.AppendLine($"Aligning graphemes [{_lowerCased}] with phonemes [{string.Join("", _phonemes)}].");

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
                if (graphemeIdx < _graphemes.Length)
                    alignment.Enqueue(new Interval<string>(graphemeIdx, _graphemes.Length - graphemeIdx, string.Empty));

                return true;
            }

            var phoneme = _phonemes[phonemeIdx];
            var patterns = GetWrittenForms(phoneme);

            _trace.AppendLine($"Now parsing phoneme [{phoneme}].");

            bool patternMatched = false;
            foreach (var pattern in patterns)
            {
                _trace.AppendLine($"Trying pattern [{pattern}].");
                
                int length = pattern.Length;
                if (graphemeIdx + length <= _graphemes.Length &&
                    _lowerCased.Substring(graphemeIdx, length) == pattern)
                {
                    patternMatched = true;
                    _trace.AppendLine($"Pattern [{pattern}] matched.");

                    alignment.Enqueue(new Interval<string>(graphemeIdx, length, phoneme));

                    if (TryAlignNext(phonemeIdx + 1, graphemeIdx + length, ref alignment))
                        return true;
                    else
                        alignment.Dequeue();
                }
            }

            if (!patternMatched)
                _trace.AppendLine($"Could not parse phoneme [{phoneme}]: backtracking.");

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
            { "s", new string[] { "ss", "s", "c", "t", "ç", "x" }},
            { "ʃ", new string[] { "ch", "sh", "ç" }},
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

            { "j", new string[] { "ill", "il", "i", "y", "j", "ï" }},
            { "w", new string[] { "hou", "ou", "ww", "wh", "w" }},
            { "ɥ", new string[] { "hu", "u" }},

            { "i", new string[] { "hi", "hy", "hï", "hî", "i", "y", "ï", "î" }},
            { "e", new string[] { "hé", "he", "ez", "ai", "aî", "é", "e" }},
            { "ɛ", new string[] { "hai", "he", "hè", "hê", "hë", "ai", "aî", "e", "è", "é", "ê", "ë" }},
            { "a", new string[] { "has", "ha", "hâ", "as", "aa", "a", "â" }},
            { "ɑ", new string[] { "has", "ha", "hâ", "as", "a", "â" }},
            { "ɔ", new string[] { "ho", "o" }},
            { "o", new string[] { "hau", "au", "ho", "hô", "o", "ô" }},
            { "u", new string[] { "hou", "ou", "où", "oû", "hu", "u" }},
            { "y", new string[] { "hu", "hû", "hü", "u", "û", "ü" }},
            { "ø", new string[] { "heu", "eu" }},
            { "œ", new string[] { "heu", "eu", "e" }},
            { "ə", new string[] { "he", "e" }},
            { "ɛ̃", new string [] { "hain", "hein", "hin", "hen", "ain", "ein", "in", "en" } },
            { "ɑ̃", new string [] { "han", "ham", "hen", "hem", "an", "am", "en", "em" }},
            { "ɔ̃", new string [] { "hon", "hom", "on", "om" }},
            { "œ̃", new string [] { "hun", "hum", "un", "um" }},
        };
    }
}
