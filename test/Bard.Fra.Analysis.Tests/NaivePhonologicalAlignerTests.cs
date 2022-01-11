using Bard.Fra.Glaff;
using Intervals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xunit;

namespace Bard.Fra.Analysis.Tests
{
    public class NaivePhonologicalAlignerTests
    {
        [Theory]
        [InlineData("boutique", "b:b ou:u t:t i:i qu:k e:")]
        [InlineData("cornet", "c:k o:O r:R n:n e:E t:")]
        [InlineData("anticonstitutionnellement", "an:@ t:t i:i c:k on:§ s:s t:t i:i t:t u:y t:s i:j o:o nn:n e:E ll:l e:° m:m en:@ t:")]
        // Silent 'h'
        [InlineData("hôpital", "h: ô:o p:p i:i t:t a:a l:l")]  // 'h' silent at beginning
        [InlineData("dehors", "d:d e:° h: o:O r:R s:")]        // 'h' silent inside
        [InlineData("poussah", "p:p ou:u ss:s a:a h:")]        // 'h' silent at the end
        // Digram with 'h'
        [InlineData("chat", "ch:S a:a t:t")]                   // french 'ch' digram
        [InlineData("philosophe", "ph:f i:i l:l o:o s:z o:O ph:f e:")]  // greek 'ph' digram
        [InlineData("chianti", "ch:k i:i an:@ t:t i:i")]       // italian 'ch' digram
        [InlineData("vecchio", "v:v e:e cch:k i:i o:o")]       // italian 'cch' digram
        [InlineData("bouddha", "b:b ou:u ddh:d a:a")]       // italian 'cch' digram
        [InlineData("dharma", "dh:d a:a r:R m:m a:a")]       // italian 'cch' digram
        [InlineData("pha", "ph:p a:a")]       // italian 'cch' digram
        [InlineData("ppha", "pph:p a:a")]       // italian 'cch' digram
        [InlineData("tha", "th:t a:a")]       // italian 'cch' digram
        [InlineData("ttha", "tth:t a:a")]       // italian 'cch' digram
        [InlineData("kha", "kh:k a:a")]       // italian 'cch' digram
        [InlineData("kkha", "kkh:k a:a")]       // italian 'cch' digram
        public void TestCompute(string graphemes, string rawAlignement)
        {
            var alignment = ParseAlignement(rawAlignement).ToArray();
            var phonemes = alignment.SelectMany(a => a.phonemes).ToArray();

            var aligner = new NaivePhonologicalAligner(graphemes, phonemes);
            var result = aligner.Compute();
            var trace = aligner.GetTrace();

            Assert.NotNull(result);
            Assert.Equal(alignment.Length, result.Length);

            for (int i = 0; i < result.Length; i++)
            {
                var realInterval = result[i];
                var realGraphemes = graphemes.Substring(realInterval.Start, realInterval.Length);
                var realPhonemes = string.Join("", realInterval.Value);

                var expectedInterval = alignment[i];

                Assert.Equal(expectedInterval.graphemes, realGraphemes);
                Assert.Equal(string.Join("", expectedInterval.phonemes), realPhonemes);
            }
        }


        [Fact]
        public void TestComputeGlaff()
        {
            string glaffPath = @"C:\Users\VR1\source\repos\Bard\data\glaff-1.2.2.txt";
            foreach (var entry in GlaffParser.ParseMainLexicon(glaffPath))
            {
                var graphemes = entry.GraphicalForm;
                var phonemes = IpaHelpers.ParseSymbols(entry.IpaPronunciations.Split(';')[0])
                    .Where(p => p != ".").ToArray();

                bool isFullCaps = graphemes.All(c => Char.IsUpper(c));

                if (isFullCaps || phonemes.Length == 0)
                    continue;

                var aligner = new NaivePhonologicalAligner(graphemes, phonemes);
                Interval<string>[] alignment = null;
                try
                {
                    alignment = aligner.Compute();
                    var trace = aligner.GetTrace();

                    if (alignment == null)
                    {
                    }
                }
                catch (Exception e)
                {
                }


                Assert.NotNull(alignment);
            }
        }


        private IEnumerable<(string graphemes, string[] phonemes)> ParseAlignement(string rawAlignment)
        {
            foreach (var interval in rawAlignment.Split(' '))
            {
                var parts = interval.Split(':');
                var graphemes = parts[0];
                var phonemes = ParsePhonemes(parts[1]);

                yield return (graphemes, phonemes);
            }
        }

        private string[] ParsePhonemes(string phonemesRaw)
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

        private static Dictionary<char, string> _phonemeByChar = new Dictionary<char, string>()
        {
            { 'O', "ɔ" }, { 'E', "ɛ" }, { '°', "ə" }, { '2', "ø" }, { '9', "œ" },
            { '5', "ɛ̃" }, { '1', "œ̃" }, { '@', "ɑ̃" }, { '§', "ɔ̃" }, { '8', "ɥ" },
            { 'S', "ʃ" }, { 'Z', "ʒ" }, { 'N', "ɲ" }, { 'R', "ʁ" }, { 'x', "χ" },
            { 'G', "ŋ" },
        };
    }
}
