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

            steps.Add(new SelectBestStep());

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
            foreach (var step in _steps)
            {
                if (step.TryClean(wordForm.Pronunciation, out string cleaned, out IAnomaly anomaly))
                {
                    wordForm.Pronunciation = cleaned;
                    wordForm.PronunciationHistory.AddChange(step.Name, cleaned);
                    wordForm.Anomalies.Add(anomaly);
                }
            }
        }
    }

    public interface IPronunciationCleaningStep
    {
        string Name { get; }
        bool TryClean(string pronunciation, out string cleaned, out IAnomaly anomaly);
    }

    /// <summary>
    /// Select a single pronunciation value in case there are multiple of them
    /// </summary>
    public class SelectBestStep : IPronunciationCleaningStep
    {
        public string Name => "SelectBest";

        public bool TryClean(string pronunciation, out string cleaned, out IAnomaly anomaly)
        {
            var parts = pronunciation.Split(";", StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length <= 1)
            {
                cleaned = null;
                anomaly = default;

                return false;
            }
            else
            {
                cleaned = parts.First();
                anomaly = new GenericAnomaly(AnomalyType.MultiplePronunciations);
                return true;
            }
        }
    }
}
