using Bard.Contracts.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bard.Fra.Analysis.Phonology.Tests
{
    public class SyllabifierTests : TestBase
    {

        [Theory]
        [InlineData("uR.s5")]
        public void TestCompute(string phonemes)
        {
            var syllables = phonemes.Split('.').Select(s => ParsePhonemes(s));
            var expected = string.Join(".", syllables.Select(s => string.Join(string.Empty, s)));
            var input = syllables.SelectMany(s => s)
                .Select(p => Phonemes.BySymbol(p)).ToArray();

            var result = new Syllabifier().Compute(input).ToArray();

            Assert.Equal(expected, result.Format());
        }

        [Fact]
        public void TestCompute2()
        {
            var syllabifier = new Syllabifier();

            var phonemes = "stʁil.bjuʁ.goz.dʁɥi.fwastχ";
            var input = string.Join("", phonemes.Split('.')).ToCharArray().Select(c => Phonemes.BySymbol(c.ToString())).ToArray();

            var expected = new[]
            {
                new Syllable(
                    onset: new [] { Phonemes.s, Phonemes.t, Phonemes.R, },
                    nucleus: Phonemes.i,
                    coda: new [] { Phonemes.l, }),
                new Syllable(
                    onset: new [] { Phonemes.b, Phonemes.j, },
                    nucleus: Phonemes.u,
                    coda: new [] { Phonemes.R, }),
                new Syllable(
                    onset: new [] { Phonemes.g, },
                    nucleus: Phonemes.o,
                    coda: new [] { Phonemes.z, }),
                new Syllable(
                    onset: new [] { Phonemes.d, Phonemes.R, Phonemes.Y, },
                    nucleus: Phonemes.i,
                    coda: new Phoneme[0]),
                new Syllable(
                    onset: new [] { Phonemes.f, Phonemes.w, },
                    nucleus: Phonemes.a,
                    coda: new [] { Phonemes.s, Phonemes.t, Phonemes.X, }),
            };

            var result = syllabifier.Compute(input).ToArray();

            Assert.Equal(expected.Length, result.Length);
        }
    }
}
