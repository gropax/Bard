﻿using Intervals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bard.Fra.Analysis.Phonology
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

        public Interval<string[]>[] Compute()
        {
            _trace = new StringBuilder();
            _trace.AppendLine($"Aligning graphemes [{_lowerCased}] with phonemes [{string.Join("", _phonemes)}].");

            var alignments = new Stack<Interval<string[]>>();
            if (TryAlignNext(0, 0, ref alignments))
                return alignments.Reverse().ToArray();
            else
                return null;
        }

        private char[] _nonPalatalLetters = new[] { 'a', 'o', 'u', 'ü' };

        private bool TryAlignNext(int phonemeIdx, int graphemeIdx, ref Stack<Interval<string[]>> alignment)
        {
            // If all phonemes are already parsed, consider the remaining graphemes as silent ending
            int remainingPhonemes = _phonemes.Length - phonemeIdx;
            if (remainingPhonemes == 0)
            {
                if (graphemeIdx < _graphemes.Length)
                    alignment.Push(new Interval<string[]>(graphemeIdx, _graphemes.Length - graphemeIdx, new string[0]));

                return true;
            }

            // If all graphemes are already parsed and there remains phonemes, the alignment fails
            if (graphemeIdx == _lowerCased.Length)
            {
                _trace.AppendLine($"Could not parse phoneme [{_phonemes[phonemeIdx]}]: no remaing graphemes.");
                return false;
            }

            string nextGraphemes = _lowerCased.Substring(graphemeIdx);
            string nextPhonemes = string.Join(string.Empty, _phonemes[phonemeIdx..]);

            // If next grapheme is 'h', consider it as silent
            if (nextGraphemes.StartsWith('h'))
            {
                alignment.Push(new Interval<string[]>(graphemeIdx, 1, new string[0]));
                return TryAlignNext(phonemeIdx, graphemeIdx + 1, ref alignment);
            }

            // Handle multi-phonemes combinations
            if (remainingPhonemes >= 2)
            {
                // If next grapheme is 'x'
                if (nextGraphemes.StartsWith('x') && nextPhonemes.StartsWith("ks"))
                {
                    alignment.Push(new Interval<string[]>(graphemeIdx, 1, new[] { "k", "s" }));
                    return TryAlignNext(phonemeIdx + 2, graphemeIdx + 1, ref alignment);
                }
                else if (nextGraphemes.StartsWith('x') && nextPhonemes.StartsWith("gz"))
                {
                    alignment.Push(new Interval<string[]>(graphemeIdx, 1, new[] { "g", "z" }));
                    return TryAlignNext(phonemeIdx + 2, graphemeIdx + 1, ref alignment);
                }
                else if (nextGraphemes.StartsWith("oin") && nextPhonemes.StartsWith("wɛ̃"))
                {
                    alignment.Push(new Interval<string[]>(graphemeIdx, 3, new[] { "w", "ɛ̃" }));
                    return TryAlignNext(phonemeIdx + 2, graphemeIdx + 3, ref alignment);
                }
                else if (nextGraphemes.StartsWith("oi") && nextPhonemes.StartsWith("wa"))
                {
                    alignment.Push(new Interval<string[]>(graphemeIdx, 2, new[] { "w", "a" }));
                    return TryAlignNext(phonemeIdx + 2, graphemeIdx + 2, ref alignment);
                }
                else if (nextGraphemes.StartsWith("zz") && nextPhonemes.StartsWith("dz"))
                {
                    alignment.Push(new Interval<string[]>(graphemeIdx, 2, new[] { "d", "z" }));
                    return TryAlignNext(phonemeIdx + 2, graphemeIdx + 2, ref alignment);
                }
                else if (nextGraphemes.StartsWith("zz") && nextPhonemes.StartsWith("tz"))
                {
                    alignment.Push(new Interval<string[]>(graphemeIdx, 2, new[] { "t", "z" }));
                    return TryAlignNext(phonemeIdx + 2, graphemeIdx + 2, ref alignment);
                }

                // Handle special case of 'ge' followed by a non palatal vowel (mangeais)
                if (
                    nextGraphemes.Length >= 3 && nextGraphemes.StartsWith("ge") &&
                    nextPhonemes.StartsWith("ʒ") &&
                    _nonPalatalLetters.Contains(nextGraphemes[2]))
                {
                    alignment.Push(new Interval<string[]>(graphemeIdx, 2, new[] { "ʒ" }));
                    return TryAlignNext(phonemeIdx + 1, graphemeIdx + 2, ref alignment);
                }
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

                    alignment.Push(new Interval<string[]>(graphemeIdx, length, new[] { phoneme }));

                    if (TryAlignNext(phonemeIdx + 1, graphemeIdx + length, ref alignment))
                        return true;
                    else
                        alignment.Pop();
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

        private Dictionary<string, string[]> _multiPhonemeDigrams = new Dictionary<string, string[]>()
        {
            { "p", new string[] { "pph", "pp", "ph", "p" } },
        };

        private Dictionary<string, string[]> _writtenForms = new Dictionary<string, string[]>()
        {
            { "p", new string[] { "pph", "pp", "ph", "p" } },
            { "t", new string[] { "tth", "tt", "th", "pt", "t" }},
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
            { "ʁ", new string[] { "rr", "r" }},
            { "m", new string[] { "mm", "m" }},
            { "n", new string[] { "nn", "mn", "n" }},
            { "ɲ", new string[] { "gn" }},
            { "ŋ", new string[] { "ng" }},
            { "x", new string[] { "j" }},

            { "j", new string[] { "ill", "il", "i", "y", "j", "ï" }},
            { "w", new string[] { "ou", "ww", "wh", "w" }},
            { "ɥ", new string[] { "u" }},

            { "i", new string[] { "i", "y", "ï", "î" }},
            { "e", new string[] { "ai", "aî", "ae", "oe", "é", "e", "æ", "œ" }},
            { "ɛ", new string[] { "ai", "aî", "ei", "ae", "e", "è", "é", "ê", "ë", "a", "æ" }},
            { "a", new string[] { "aa", "a", "â" }},
            { "ɑ", new string[] { "a", "â" }},
            { "ɔ", new string[] { "o" }},
            { "o", new string[] { "eau", "au", "o", "ô" }},
            { "u", new string[] { "ou", "où", "oû", "u" }},
            { "y", new string[] { "u", "û", "ü" }},
            { "ø", new string[] { "oeu", "eu", "œu", "oe", "œ" }},
            { "œ", new string[] { "oeu", "eu", "œu", "oe", "e", "œ" }},
            { "ə", new string[] { "e", "ë" }},
            { "ɛ̃", new string [] { "ain", "ein", "in", "en" } },
            { "ɑ̃", new string [] { "aon", "an", "am", "en", "em" }},
            { "ɔ̃", new string [] { "on", "om" }},
            { "œ̃", new string [] { "un", "um" }},
        };
    }
}
