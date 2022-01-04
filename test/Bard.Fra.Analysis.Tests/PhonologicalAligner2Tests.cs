using System;
using Xunit;

namespace Bard.Fra.Analysis.Tests
{
    public class PhonologicalAligner2Tests
    {
        [Theory]
        [InlineData("boutique", "b u t i k", "b:b ou:u t:t i:i qu:k e:")]
        public void TestCompute(string graphemes, string phonemesRaw, string expectedRaw)
        {
            var phonemes = phonemesRaw.Split(' ');
            var aligner = new PhonologicalAligner2(graphemes, phonemes);
            var result = aligner.Compute();

            if (expectedRaw == null)
                Assert.Null(result);
            else
            {
                var expectedIntervalsRaw = expectedRaw.Split(' ');

                Assert.Equal(expectedIntervalsRaw.Length, result.Length);

                for (int i = 0; i < result.Length; i++)
                {
                    var interval = result[i];
                    var realGraphemes = graphemes.Substring(interval.Start, interval.Length);
                    var realPhonemes = string.Join("", interval.Value);

                    var parts = expectedIntervalsRaw[i].Split(':');
                    var expectedGraphemes = parts[0];
                    var expectedPhonemes = parts[1];

                    Assert.Equal(expectedGraphemes, realGraphemes);
                    Assert.Equal(expectedPhonemes, realPhonemes);
                }
            }
        }
    }
}
