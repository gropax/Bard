using Intervals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bard.Fra.Analysis
{
    public class PhonologicalAligner
    {
        private string _written;
        private string[] _phonemes;

        public PhonologicalAligner(string written, string[] phonemes)
        {
            _written = written;
            _phonemes = phonemes;
        }

        private int _writtenIdx;
        private int _phonemeIdx;
        private List<Interval<string>> _alignments;

        /// <summary>
        /// Hypothesis:
        /// - no gemminated consonant phonemes
        /// </summary>
        /// <param name="written"></param>
        /// <param name="phonemes"></param>
        /// <returns></returns>
        public IEnumerable<Interval<string>> Compute()
        {
            _writtenIdx = 0;
            _phonemeIdx = 0;
            _alignments = new List<Interval<string>>();

            int start = 0;

            var phonemeGroup = new List<string[]>();

            int plen = _phonemes.Length;
            while (_phonemeIdx < plen - 1)
            {
                string phoneme = _phonemes[_phonemeIdx];

                if (_written[_writtenIdx] == 'x')
                {
                    // Only case where one graphical character is aligned with several phonemes
                }
                else
                {
                    TryMatchGraphical(phoneme, _phonemeRegexes[phoneme]);
                }
            }

            _alignments.Add(new Interval<string>(_writtenIdx, _written.Length - _writtenIdx, value: _phonemes.Last()));

            return _alignments;
        }


        private Dictionary<string, Regex> _phonemeRegexes = new Dictionary<string, Regex>()
        {
            { "p", new Regex(@"^pp?") },
            { "t", new Regex(@"^tt?h?") },
            { "k", new Regex(@"^kk?h?|cc?|qu") },
            { "b", new Regex(@"^bb?") },
            { "d", new Regex(@"^dd?h?") },
            { "g", new Regex(@"^gg?(?:h|u)?") },
            { "f", new Regex(@"^ff?|ph") },
            { "s", new Regex(@"^ss?|x") },
            { "ʃ", new Regex(@"^(?:c|s)h") },
            { "v", new Regex(@"^v|w") },
            { "z", new Regex(@"^s|z") }, // x?
            { "ʒ", new Regex(@"^jj?|gg?") },
            { "l", new Regex(@"^ll?") },
            { "ʁ", new Regex(@"^rr?") },
            { "m", new Regex(@"^mm?") },
            { "n", new Regex(@"^nn?") },
            { "ɲ", new Regex(@"^gn") },
            { "ŋ", new Regex(@"^ng") },
            { "x", new Regex(@"^j") },

            { "j", new Regex(@"^y|ï|ill") },
            { "w", new Regex(@"^w|ou") },
            { "ɥ", new Regex(@"^u") },

            { "i", new Regex(@"^i|y") },
            { "e", new Regex(@"^é") },
            { "ɛ", new Regex(@"^è|ê|ë|ai") },
            { "a", new Regex(@"^a|â") },
            { "ɑ", new Regex(@"^a|â") },
            { "ɔ", new Regex(@"^o") },
            { "o", new Regex(@"^o|au") },
            { "u", new Regex(@"^o?u") },
            { "y", new Regex(@"^u") },
            { "ø", new Regex(@"^eu") },
            { "œ", new Regex(@"^eu") },
            { "ə", new Regex(@"^e") },
            { "ɛ̃", new Regex(@"^(?:a|e)i(?:n|m)") },
            { "ɑ̃", new Regex(@"^(?:a|e)(?:n|m)") },
            { "ɔ̃", new Regex(@"^o(?:n|m)") },
            { "œ̃", new Regex(@"^u(?:n|m)") },
        };

        private void TryMatchGraphical(string phoneme, Regex regex)
        {
            var match = regex.Match(_written[_writtenIdx..]);
            if (match.Success)
            {
                var len = match.Groups[0].Value.Length;

                _alignments.Add(new Interval<string>(
                    start: _writtenIdx,
                    length: len,
                    value: phoneme));

                _writtenIdx += len;
                _phonemeIdx++;
            }
            else
                throw new Exception($"Could not align phoneme /{phoneme}/ at index {_writtenIdx}: {_written[.._writtenIdx]}|{_written[_writtenIdx..]} (looking for pattern /{regex}/).");
        }
    }
}
