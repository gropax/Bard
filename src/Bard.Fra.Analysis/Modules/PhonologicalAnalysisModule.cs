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

            FixPhonemes(wordForm);

            var stdReal = GetStandardRealization(wordForm);
            var allReals = DeriveAllPossibleRealizations(stdReal);

            wordForm.StdRealization = stdReal;
            wordForm.Realizations = allReals;
        }

        private void FixPhonemes(WordForm wordForm)
        {
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
        }

        private PhoneticRealization GetStandardRealization(WordForm wordForm)
        {
            var phonSeq = GetPhoneticSequence(wordForm.Phonemes);

            return new PhoneticRealization()
            {
                Graphemes = wordForm.GlaffEntry.GraphicalForm,
                Alignment = wordForm.Alignment,
                PhoneticSequence = phonSeq,
                IsStandard = true,
            };
        }

        private Dictionary<string, PhoneticSequence> _phonSeqs = new Dictionary<string, PhoneticSequence>();
        private PhoneticSequence GetPhoneticSequence(Phoneme[] phonemes)
        {
            string key = phonemes.Format();

            if (!_phonSeqs.TryGetValue(key, out var phonSeq))
            {
                var syllables = new Syllabifier().Compute(phonemes).ToArray();
                var rhymes = GetAllRhymes(syllables);

                phonSeq = new PhoneticSequence()
                {
                    Id = phonemes.Format(),
                    Phonemes = phonemes,
                    Syllables = syllables,
                    Rhymes = rhymes,
                };
                _phonSeqs[key] = phonSeq;
            }

            return phonSeq;
        }

        private Rhyme[] GetAllRhymes(Syllable[] syllables)
        {
            var rhymes = new List<Rhyme>();

            // Add final rhyme
            rhymes.Add(new Rhyme()
            {
                Phonemes = syllables.Last().Phonemes.ToArray(),
                IsFinal = true,
            });

            // Add inner rhymes
            for (int i = 0; i < syllables.Length; i++)
            {
                var syllable = syllables[i];
                rhymes.Add(new Rhyme()
                {
                    Phonemes = syllable.Phonemes.ToArray(),
                    IsFinal = false,
                    SyllableNumber = i + 1,
                });
            }

            return rhymes.ToArray();
        }

        private PhoneticRealization[] DeriveAllPossibleRealizations(PhoneticRealization stdReal)
        {
            return new[] { stdReal };
        }

        private Dictionary<Phoneme, Phoneme> _vowelCorrections = new Dictionary<Phoneme, Phoneme>()
        {
            { Phonemes.E, Phonemes.e }, { Phonemes.O, Phonemes.o }, { Phonemes.oe, Phonemes.eu },
        };
    }
}
