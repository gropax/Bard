using Bard.Contracts.Fra;
using Bard.Fra.Analysis.Phonology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public static class PronunciationCleaningModuleFactory
    {
        public class Config
        {
            public bool Enabled { get; set; } = true;
        }

        public static PronunciationCleaningModule Build(Config config)
        {
            var steps = new List<IPhonologicalAnalyzer>();

            steps.Add(new AlignmentAnalyzer());
            steps.Add(new FixSyllabationStep());

            return new PronunciationCleaningModule(steps.ToArray());
        }
    }

    public class PronunciationCleaningModule : IAnalysisModule
    {
        private IPhonologicalAnalyzer[] _steps;

        public PronunciationCleaningModule(IPhonologicalAnalyzer[] steps)
        {
            _steps = steps;
        }

        public void Analyze(WordForm wordForm)
        {
            // Separate the multiple pronunciations
            var pronunciations = ParsePronunciations(wordForm);

            // Run each in the pipeline
            foreach (var step in _steps)
            {
                var newValues = new List<string>();
                bool hasChanged = false;

                foreach (var pronunciation in pronunciations)
                {
                    if (step.Analyze(pronunciation))
                    {
                        hasChanged = true;
                    }

                    newValues.Add(pronunciation.Value);
                }

                if (hasChanged)
                    wordForm.PronunciationHistory.AddChange(step.Name, string.Join(";", newValues));
            }

            // Pick the one with the least anomalies
            var best = pronunciations
                .Where(p => p.IsValid)
                .OrderBy(p => p.Anomalies.Count).FirstOrDefault();

            if (best != null)
            {
                wordForm.Pronunciation = best.Value;
                wordForm.Phonemes = best.Phonemes.Select(p => Phonemes.BySymbol(p)).ToArray();
                wordForm.Alignment = best.Alignment;
                wordForm.Anomalies.AddRange(best.Anomalies);

                if (pronunciations.Length > 1)
                {
                    wordForm.PronunciationHistory.AddChange("SelectBest", best.Value);
                    wordForm.Anomalies.Add(new GenericAnomaly(AnomalyType.MultiplePronunciations));
                }
            }
        }

        private Pronunciation[] ParsePronunciations(WordForm wordForm)
        {
            var parts = wordForm.Pronunciation.Split(";", StringSplitOptions.RemoveEmptyEntries);
            return parts.Select(p => new Pronunciation(wordForm.GlaffEntry.GraphicalForm, p)).ToArray();
        }
    }

    public interface IPhonologicalAnalyzer
    {
        string Name { get; }
        bool Analyze(Pronunciation pronunciation);
    }

    /// <summary>
    /// Fix missing syllable boundary between multiple vowels
    /// </summary>
    public class FixSyllabationStep : IPhonologicalAnalyzer
    {
        public string Name => "FixSyllabation";

        public bool Analyze(Pronunciation pronunciation)
        {
            string pronuncValue = pronunciation.Value;

            if (string.IsNullOrWhiteSpace(pronuncValue))
                return false;

            // Determine index of missing syllable separators in the pronunciation string
            var missingDotIndices = FindMissingDots(pronuncValue);
            if (missingDotIndices.Count == 0)
                return false;
            else
            {
                var builder = new StringBuilder();

                int start = 0;
                foreach (var idx in missingDotIndices)
                {
                    builder.Append(pronuncValue.Substring(start, idx - start));
                    builder.Append('.');
                    start = idx;
                }
                builder.Append(pronuncValue.Substring(start, pronuncValue.Length - start));

                pronunciation.Value = builder.ToString();
                pronunciation.Anomalies.Add(new GenericAnomaly(AnomalyType.BadSyllabation));

                return true;
            }
        }

        private List<int> FindMissingDots(string pronunciation)
        {
            var missingDotIndices = new List<int>();

            var symbols = IpaHelpers.ParseSymbols(pronunciation);
            int len = symbols.Length;
            if (len == 1)
                return missingDotIndices;

            string current = symbols[0];
            int idx = current.Length;
            bool currentIsVowel = IpaHelpers.IsVowel(current);

            for (int i = 1; i < len; i++)
            {
                string next = symbols[i];
                bool nextIsVowel = IpaHelpers.IsVowel(next);

                if (currentIsVowel && nextIsVowel)
                    missingDotIndices.Add(idx);

                currentIsVowel = nextIsVowel;
                idx += next.Length;
            }

            return missingDotIndices;
        }
    }

    public class AlignmentAnalyzer : IPhonologicalAnalyzer
    {
        public string Name => "PhonologicalAlignment";

        public bool Analyze(Pronunciation pronunciation)
        {
            var graphemes = pronunciation.Graphemes;

            // Flatten syllabized phoneme list
            var phonemesStr = pronunciation.Value.Replace(".", string.Empty);
            var phonemes = IpaHelpers.ParseSymbols(phonemesStr)
                .Where(p => Phonemes.IsValid(p))
                .ToArray();

            if (phonemes.Length == 0)
            {
                pronunciation.IsValid = false;
                return true;
            }

            pronunciation.Phonemes = phonemes;

            // Align phonemes with graphemes
            //
            var aligner = new NaivePhonologicalAligner(graphemes, phonemes);
            var alignment = aligner.Compute();
            if (alignment == null)
                pronunciation.Anomalies.Add(new GenericAnomaly(AnomalyType.AlignmentFailed));
            else
                pronunciation.Alignment = string.Join(" ", alignment.Select(i => $"{graphemes.Substring(i.Start, i.Length)}:{string.Join(string.Empty, i.Value)}"));

            return false;
        }
    }
}
