using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Glaff.Modules
{
    public class MissingPronunciationDetectionConfig
    {
        public bool Enabled { get; set; } = true;
        public bool AbortIfMissing { get; set; } = false;
    }

    public class MissingPronunciationDetectionModule : IAnalysisModule<GlaffEntry>
    {
        public MissingPronunciationDetectionConfig Config { get; }

        public MissingPronunciationDetectionModule(MissingPronunciationDetectionConfig config)
        {
            Config = config;
        }

        public bool Analyze(AnalysisResult<GlaffEntry> result)
        {
            bool isMissing = string.IsNullOrWhiteSpace(result.Result.IpaPronunciations);

            result.Result.MissingPronunciation = isMissing;

            bool abort = isMissing && Config.AbortIfMissing;
            return abort;
        }
    }
}
