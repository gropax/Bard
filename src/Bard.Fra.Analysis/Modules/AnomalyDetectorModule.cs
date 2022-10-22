using Bard.Contracts.Fra;
using Bard.Fra.Analysis.Phonology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public static class AnomalyDetectorModuleFactory
    {
        public class Config
        {
            public bool Enabled { get; set; } = true;
            public bool DetectAcronyms { get; set; } = true;
            public bool DetectNoPhonemes { get; set; } = true;
        }

        public static AnomalyDetectorModule Build(Config config)
        {
            var filters = new List<AnomalyDetector>();

            if (config.DetectNoPhonemes)
                filters.Add(new AnomalyDetector(AnomalyType.NoPhoneme,
                    (wordForm) => string.IsNullOrWhiteSpace(wordForm.GlaffEntry.IpaPronunciations)));

            if (config.DetectAcronyms)
                filters.Add(new AnomalyDetector(AnomalyType.Acronym, (wordForm) =>
                {
                    var graphemes = wordForm.GlaffEntry.GraphicalForm;
                    return graphemes.Length > 2 && Char.IsUpper(graphemes[0]) && Char.IsUpper(graphemes[1]);
                }));

            return new AnomalyDetectorModule(filters.ToArray());
        }
    }

    public class AnomalyDetector
    {
        public AnomalyType Type { get; }
        public Func<WordForm, bool> Condition { get; }
        public AnomalyDetector(AnomalyType type, Func<WordForm, bool> condition)
        {
            Type = type;
            Condition = condition;
        }
    }

    public class AnomalyDetectorModule : IAnalysisModule
    {
        private AnomalyDetector[] _detectors;

        public AnomalyDetectorModule(AnomalyDetector[] detectors)
        {
            _detectors = detectors;
        }

        public void Analyze(WordForm wordForm)
        {
            foreach (var detector in _detectors)
            {
                if (detector.Condition(wordForm))
                {
                    wordForm.Anomalies.Add(new GenericAnomaly(detector.Type));

                    //wordForm.IsValid = false;
                    //return;
                }
            }
        }
    }
}
