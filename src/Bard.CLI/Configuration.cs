using Bard.Fra.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.CLI
{
    class Configuration
    {
        public LexiconConfig Lexicons { get; set; }
        public GraphStorageConfig GraphStorage { get; set; }
        public AnalysisPipelineConfig Analysis { get; set; }
    }

    class LexiconConfig
    {
        public string Main { get; set; }
        public string Oldies { get; set; }
        public int Limit { get; set; } = int.MaxValue;
        public int BatchSize { get; set; }
    }

    class GraphStorageConfig
    {
        public string Address { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int BatchSize { get; set; }
    }

    class AnalysisPipelineConfig
    {
        public AnomalyDetectorModuleFactory.Config AnomalyDetectorModule { get; set; }
        public PronunciationCleaningModuleFactory.Config PronunciationCleaningModule { get; set; }
        public PhonologicalAnalysisModuleFactory.Config PhonologicalAnalysisModule { get; set; }
        public LemmaDetectionModuleFactory.Config LemmaDetectionModule { get; set; }
    }
}
