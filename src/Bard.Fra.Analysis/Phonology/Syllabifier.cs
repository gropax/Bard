using Bard.Contracts.Fra;
using Bard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    /// <summary>
    /// https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&cad=rja&uact=8&ved=2ahUKEwjApIuHwvHvAhVBgRoKHfjsC0IQFjAAegQIBRAD&url=https%3A%2F%2Fjeremy-pasquereau.jimdofree.com%2Fapp%2Fdownload%2F18807748525%2FIntroLingCM.pdf%3Ft%3D1606407744&usg=AOvVaw2CxSupS1-u7nILanf5Cti4
    /// </summary>
    public class Syllabifier
    {
        public IEnumerable<Syllable> Compute(IEnumerable<Phoneme> phonemes)
        {
            var blocks = SeparateVowels(phonemes).ToArray();

            Phoneme[] onset = null;
            Phoneme nucleus = null;
            //Phoneme[] coda = null;

            for (int i = 0; i < blocks.Length; i++)
            {
                var block = blocks[i];

                if (i == 0)
                    onset = block;
                else if (i == blocks.Length - 1)
                    yield return new Syllable(onset, nucleus, block);
                else if (i % 2 == 1)  // vowel
                    nucleus = block.Single();
                else
                {
                    (var coda, var nextOnset) = SeparateConsonantCluster(block);
                    yield return new Syllable(onset, nucleus, coda);

                    onset = nextOnset;
                }
            }
        }

        private (Phoneme[], Phoneme[]) SeparateConsonantCluster(Phoneme[] consonants)
        {
            for (int i = 0; i < consonants.Length; i++)
            {
                var onset = consonants[i..^0];
                if (IsValidOnset(onset))
                {
                    var coda = consonants[0..i];
                    return (coda, onset);
                }
            }
            return (consonants, new Phoneme[0]);
        }

        private bool IsValidOnset(Phoneme[] consonants)
        {
            return consonants.ConsecutivePairs((c1, c2) => Score(c1) < Score(c2)).All(b => b);
        }

        private int Score(Phoneme p)
        {
            if (!p.IsContinuous && !p.IsSyllabic)  // Stops
                return 0;
            else if (p.IsSyllabic)  // Nasal and liquids
                return 1;
            else if (p.IsConsonantic && p.IsContinuous)  // Fricatives
                return 2;
            else  // Semi vowels
                return 3;
        }

        private IEnumerable<Phoneme[]> SeparateVowels(IEnumerable<Phoneme> phonemes)
        {
            var tmp = new List<Phoneme>();

            foreach (var phoneme in phonemes)
            {
                if (phoneme.IsVowel)
                {
                    yield return tmp.ToArray();
                    yield return new[] { phoneme };
                    tmp.Clear();
                }
                else
                    tmp.Add(phoneme);
            }
            yield return tmp.ToArray();
        }
    }

}
