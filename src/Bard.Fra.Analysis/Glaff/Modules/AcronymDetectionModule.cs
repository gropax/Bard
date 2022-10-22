using Bard.Contracts.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Glaff.Modules
{
    public class AcronymDetectionConfig
    {
        public bool Enabled { get; set; } = true;
        public bool AbortIfAcronym { get; set; } = false;
    }

    public class AcronymDetectionModule : IAnalysisModule<GlaffEntry>
    {
        public AcronymDetectionConfig Config { get; }

        public AcronymDetectionModule(AcronymDetectionConfig config)
        {
            Config = config;
        }

        public bool Analyze(AnalysisResult<GlaffEntry> result)
        {
            var graphemes = result.Result.GraphicalForm;
            bool isAcronym = graphemes.Length > 2 && Char.IsUpper(graphemes[0]) && Char.IsUpper(graphemes[1]);

            result.Result.IsAcronym = isAcronym;

            bool abort = isAcronym && Config.AbortIfAcronym;
            return abort;
        }
    }
}
