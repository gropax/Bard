using Bard.Fra.Analysis.Glaff.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Glaff
{
    public class Config
    {
        public SourceConfig Source { get; set; }
        public AnalysisConfig Analysis { get; set; }
    }

    public class SourceConfig
    {
        public string MainDataset { get; set; }
        public string OldiesDataset { get; set; }
        public int? Limit { get; set; }
        public int BatchSize { get; set; }
    }

    public class AnalysisConfig
    {
        public MissingPronunciationDetectionConfig MissingPronunciationDetection { get; set; }
            = new MissingPronunciationDetectionConfig();

        public AcronymDetectionConfig AcronymDetection { get; set; }
            = new AcronymDetectionConfig();

        public LemmaDetectionConfig LemmaDetection { get; set; }
            = new LemmaDetectionConfig();
    }

}
