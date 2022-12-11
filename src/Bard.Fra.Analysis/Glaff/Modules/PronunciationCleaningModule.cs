using Bard.Contracts.Fra;
using Bard.Fra.Analysis.Phonology;
using Bard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Glaff.Modules
{
    public class PronunciationCleaningConfig
    {
        public bool Enabled { get; set; } = true;
    }

    public class PronunciationCleaningModuleFactory
    {
        private PronunciationCleaningConfig Config { get; }
        public PronunciationCleaningModuleFactory(PronunciationCleaningConfig config)
        {
            Config = config;
        }

        public PronunciationCleaningModule Build()
        {
            var steps = new List<IPhonologicalAnalyzer>();

            steps.Add(new AlignmentAnalyzer());

            steps.Add(new FixSyllabationStep());

            return new PronunciationCleaningModule(steps.ToArray());
        }
    }

    public class PronunciationCleaningModule : IAnalysisModule<GlaffEntry>
    {
        private IPhonologicalAnalyzer[] _steps;

        public PronunciationCleaningModule(IPhonologicalAnalyzer[] steps)
        {
            _steps = steps;
        }

        public bool Analyze(AnalysisResult<GlaffEntry> result)
        {
            var entry = result.Result;

            // Separate the multiple pronunciations
            var pronunciations = ParsePronunciations(entry);

            // Run each in the pipeline
            foreach (var step in _steps)
                foreach (var pronunciation in pronunciations)
                    step.Analyze(pronunciation);

            entry.Pronunciations = pronunciations
                .Where(p => p.IsValid)
                .ToArray();

            var abort = false;
            return abort;
        }

        private Pronunciation[] ParsePronunciations(GlaffEntry entry)
        {
            var parts = entry.IpaPronunciations.Split(";", StringSplitOptions.RemoveEmptyEntries);
            return parts.Select(p => new Pronunciation(entry.GraphicalForm, p)).ToArray();
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
            {
                pronunciation.BadSyllabation = true;
                return false;
            }
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
                pronunciation.History.AddChange(nameof(FixSyllabationStep), pronunciation.Value);
                pronunciation.BadSyllabation = true;

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
                pronunciation.AlignmentFailed = true;
            else
            {
                pronunciation.Alignment = string.Join(" ", alignment.Select(i => $"{graphemes.Substring(i.Start, i.Length)}:{string.Join(string.Empty, i.Value)}"));
                pronunciation.AlignmentFailed = false;
            }

            return false;
        }
    }
}
