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
        [InlineData("anticonstitutionnellement", "an:@ t:t i:i c:k on:§ s:s t:t i:i t:t u:y t:s i:j o:o nn:n e:E ll:l e:° m:m en:@ t:")]
        // /p/ sound
        [InlineData("pomme", "p:p o:O mm:m e:")]
        [InlineData("frappe", "f:f r:R a:a pp:p e:")]
        [InlineData("pha", "ph:p a:a")]
        [InlineData("ppha", "pph:p a:a")]
        // /b/ sound
        [InlineData("bourde", "b:b ou:u r:R d:d e:")]
        [InlineData("abbé", "a:a bb:b é:e")]
        [InlineData("abhorré", "a:a bh:b o:o rr:R é:e")]
        [InlineData("bbha", "bbh:b a:a")]
        // /t/ sound
        [InlineData("taupe", "t:t au:o p:p e:")]
        [InlineData("sculpter", "s:s c:k u:y l:l pt:t e:e r:")]
        [InlineData("thé", "th:t é:e")]
        [InlineData("matthéen", "m:m a:a tth:t é:e en:5")]
        // /d/ sound
        [InlineData("dur", "d:d u:y r:R")]
        [InlineData("addict", "a:a dd:d i:i c:k t:t")]
        [InlineData("bouddha", "b:b ou:u ddh:d a:a")]
        [InlineData("dharma", "dh:d a:a r:R m:m a:a")]
        // /k/ sound
        [InlineData("cadeaux", "c:k a:a d:d eau:o x:")]
        [InlineData("accroc", "a:a cc:k r:R o:o c:")]
        [InlineData("cinq", "c:s in:5 q:k")]
        [InlineData("qatar", "q:k a:a t:t a:a r:R")]
        [InlineData("quinze", "qu:k in:5 z:z e:")]
        [InlineData("chianti", "ch:k i:i an:@ t:t i:i")]
        [InlineData("vecchio", "v:v e:e cch:k i:i o:o")]
        [InlineData("akène", "a:a k:k è:E n:n e:")]
        [InlineData("trekking", "t:t r:R e:E kk:k i:i ng:G")]
        [InlineData("cheikh", "ch:S ei:E kh:k")]
        [InlineData("kha", "kh:k a:a")]
        [InlineData("kkha", "kkh:k a:a")]
        // /g/ sound
        // Silent endings
        [InlineData("cornet", "c:k o:O r:R n:n e:E t:")]
        [InlineData("manger", "m:m an:@ g:Z e:e r:")]
        [InlineData("mange", "m:m an:@ g:Z e:")]
        [InlineData("manges", "m:m an:@ g:Z es:")]
        [InlineData("mangent", "m:m an:@ g:Z ent:")]
        [InlineData("jonc", "j:Z on:§ c:")]
        [InlineData("plomb", "p:p l:l om:§ b:")]
        [InlineData("lard", "l:l a:a r:R d:")]
        [InlineData("clef", "c:k l:l e:e f:")]
        // Silent 'h'
        [InlineData("hôpital", "h: ô:o p:p i:i t:t a:a l:l")]  // 'h' silent at beginning
        [InlineData("dehors", "d:d e:° h: o:O r:R s:")]        // 'h' silent inside
        [InlineData("poussah", "p:p ou:u ss:s a:a h:")]        // 'h' silent at the end
        // Digram with 'h'
        [InlineData("chat", "ch:S a:a t:t")]                   // french 'ch' digram
        [InlineData("philosophe", "ph:f i:i l:l o:o s:z o:O ph:f e:")]  // greek 'ph' digram
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
