﻿using Bard.Contracts.Fra;
using Bard.Contracts.Fra.Analysis.Phonology;
using Bard.Fra.Analysis.Phonology;
using Intervals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPhonologyData = Bard.Contracts.Fra.Analysis.Phonology.WordPhonologyData;

namespace Bard.Fra.Analysis.Phonology
{
    public class PhonologicalAnalysisConfig
    {
        public bool Enabled { get; set; } = true;
    }

    public class PhonologicalAnalysisModule : IAnalysisModule<WordPhonologyData>
    {
        public PhonologicalAnalysisConfig Config { get; }

        public PhonologicalAnalysisModule(PhonologicalAnalysisConfig config)
        {
            Config = config;
        }

        public bool Analyze(AnalysisResult<WordPhonologyData> analysisResult)
        {
            var data = analysisResult.Result;

            // Select valid pronun variants among data from multiple sources
            var acceptedPronun = GetAcceptedPronunciations(data.Pronunciations);

            // Build standard realization for each pronunciation
            var stdReals = acceptedPronun.Select(p => GetStandardRealization(data.WordForm, p)).ToArray();

            // Derive all realization from standard ones
            var allReals = stdReals.SelectMany(r => DeriveAllPossibleRealizations(r)).ToArray();

            data.MainRealization = stdReals.First();
            data.Realizations = allReals;

            bool abort = false;
            return abort;
        }

        private Pronunciation[] GetAcceptedPronunciations(Pronunciation[] pronunciations)
        {
            return pronunciations.Take(1).ToArray();
        }

        private void FixPhonemes(WordPhonologyData wordForm)
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

        private PhoneticRealization GetStandardRealization(WordForm2 wordForm, Pronunciation p)
        {
            var phonWord = GetPhoneticWord(p.GetPhonemes());

            return new PhoneticRealization()
            {
                Graphemes = wordForm.GraphicalForm,
                Alignment = p.Alignment,
                PhoneticWord = phonWord,
                IsStandard = true,
            };
        }

        private Dictionary<string, PhoneticWord> _phonSeqs = new Dictionary<string, PhoneticWord>();
        private PhoneticWord GetPhoneticWord(Phoneme[] phonemes)
        {
            string key = phonemes.Format();

            if (!_phonSeqs.TryGetValue(key, out var phonWord))
            {
                var syllables = new Syllabifier().Compute(phonemes).ToArray();
                var rhymes = GetRhymes(syllables);
                var innerRhymes = GetInnerRhymes(syllables);

                phonWord = new PhoneticWord()
                {
                    PhoneticSequence = new PhoneticSequence()
                    {
                        IpaRepresentation = phonemes.Format(),
                        Phonemes = phonemes,
                        Syllables = syllables,
                    },
                    Rhymes = rhymes,
                    InnerRhymes = innerRhymes,
                };
                _phonSeqs[key] = phonWord;
            }

            return phonWord;
        }

        private Rhyme[] GetRhymes(Syllable[] syllables)
        {
            var rhymes = new List<Rhyme>();

            rhymes.Add(new Rhyme()
            {
                PhoneticSequence = new PhoneticSequence()
                {
                    IpaRepresentation = syllables.Last().Rhyme.Format(),
                    Phonemes = syllables.Last().Rhyme.ToArray(),
                    Syllables = new[] { syllables.Last().RhymeSyllable },
                },
            });

            return rhymes.ToArray();
        }


        private InnerRhyme[] GetInnerRhymes(Syllable[] syllables)
        {
            var innerRhymes = new List<InnerRhyme>();

            // Add inner rhymes
            for (int i = 0; i < syllables.Length; i++)
            {
                var syllable = syllables[i];
                innerRhymes.Add(new InnerRhyme()
                {
                    PhoneticSequence = new PhoneticSequence()
                    {
                        IpaRepresentation = syllable.Rhyme.Format(),
                        Phonemes = syllable.Rhyme.ToArray(),
                        Syllables = new[] { syllable.RhymeSyllable },
                    },
                    SyllableNumber = i + 1,
                });
            }

            return innerRhymes.ToArray();
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
