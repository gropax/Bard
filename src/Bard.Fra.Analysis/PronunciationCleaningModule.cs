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
            var steps = new List<IPronunciationCleaningStep>();

            steps.Add(new FixSyllabationStep());

            return new PronunciationCleaningModule(steps.ToArray());
        }
    }

    public class PronunciationCleaningModule : IAnalysisModule
    {
        private IPronunciationCleaningStep[] _steps;

        public PronunciationCleaningModule(IPronunciationCleaningStep[] steps)
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
                    if (step.TryClean(pronunciation.Value, out string cleaned, out IAnomaly anomaly))
                    {
                        pronunciation.Value = cleaned;
                        pronunciation.Anomalies.Add(anomaly);
                        hasChanged = true;
                    }

                    newValues.Add(pronunciation.Value);
                }

                if (hasChanged)
                    wordForm.PronunciationHistory.AddChange(step.Name, string.Join(";", newValues));
            }

            // Pick the one with the least anomalies
            var best = pronunciations.OrderByDescending(p => p.Anomalies.Count).FirstOrDefault();
            if (best != null)
            {
                wordForm.Pronunciation = best.Value;
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
            return parts.Select(p => new Pronunciation(p)).ToArray();
        }


        class Pronunciation
        {
            public string Value { get; set; }
            public ChangeHistory History { get; }
            public List<IAnomaly> Anomalies { get; } = new List<IAnomaly>();
            public Pronunciation(string value)
            {
                Value = value;
                History = new ChangeHistory(value);
            }
        }
    }

    public interface IPronunciationCleaningStep
    {
        string Name { get; }
        bool TryClean(string pronunciation, out string cleaned, out IAnomaly anomaly);
    }

    /// <summary>
    /// Fix missing syllable boundary between multiple vowels
    /// </summary>
    public class FixSyllabationStep : IPronunciationCleaningStep
    {
        public string Name => "FixSyllabation";

        public bool TryClean(string pronunciation, out string cleaned, out IAnomaly anomaly)
        {
            cleaned = default;
            anomaly = default;

            if (string.IsNullOrWhiteSpace(pronunciation))
                return false;

            // Determine index of missing syllable separators in the pronunciation string
            var missingDotIndices = FindMissingDots(pronunciation);
            if (missingDotIndices.Count == 0)
                return false;
            else
            {
                var builder = new StringBuilder();

                int start = 0;
                foreach (var idx in missingDotIndices)
                {
                    builder.Append(pronunciation.Substring(start, idx - start));
                    builder.Append('.');
                    start = idx;
                }
                builder.Append(pronunciation.Substring(start, pronunciation.Length - start));

                cleaned = builder.ToString();
                anomaly = new GenericAnomaly(AnomalyType.BadSyllabation);

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
}
