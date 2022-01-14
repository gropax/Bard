using Bard.Fra.Analysis.Phonology;
using Intervals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public static class PhonologicalAnalysisModuleFactory
    {
        public class Config
        {
            public bool Enabled { get; set; } = true;
        }

        public static PhonologicalAnalysisModule Build(Config config)
        {
            return new PhonologicalAnalysisModule();
        }
    }

    public class PhonologicalAnalysisModule : IAnalysisModule
    {
        public void Analyze(WordForm wordForm)
        {
            if (wordForm.Phonemes == null)
                return;

            var graphemes = wordForm.GlaffEntry.GraphicalForm;

            wordForm.Syllables = new Syllabifier().Compute(wordForm.Phonemes).ToArray();
            if (wordForm.Syllables.Length == 0)
            {
                wordForm.Anomalies.Add(new GenericAnomaly(AnomalyType.SyllabificationError));
                return;
            }

            // Fix misuse of /ɛ/, /ɔ/ and /œ/ in non-final open syllables
            var newSyllables = new List<Syllable>();
            foreach (var syllable in wordForm.Syllables[..^1])
            {
                if (syllable.IsOpen && _vowelCorrections.TryGetValue(syllable.Nucleus, out var correction))
                    newSyllables.Add(new Syllable(syllable.Onset, correction, syllable.Coda));
                else
                    newSyllables.Add(syllable);
            }
            newSyllables.Add(wordForm.Syllables.Last());

            wordForm.Syllables = newSyllables.ToArray();
            wordForm.Phonemes = newSyllables.SelectMany(s => s.Phonemes).ToArray();

            wordForm.Rhymes = new Rhymes()
            {
                FinalRhyme = wordForm.Syllables.Last().Rhyme,
                InnerRhymes = wordForm.Syllables.Select(s => s.Rhyme).ToArray(),
            };
        }

        private Dictionary<Phoneme, Phoneme> _vowelCorrections = new Dictionary<Phoneme, Phoneme>()
        {
            { Phonemes.E, Phonemes.e }, { Phonemes.O, Phonemes.o }, { Phonemes.oe, Phonemes.eu },
        };
    }
}
