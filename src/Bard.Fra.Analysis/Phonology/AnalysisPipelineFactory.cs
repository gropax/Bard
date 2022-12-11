using Bard.Contracts.Fra.Analysis.Phonology;
using Bard.Fra.Analysis;
using Bard.Fra.Analysis.Glaff.Modules;
using Bard.Fra.Analysis.Words.Nouns.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    public class AnalysisConfig
    {
        public PhonologicalAnalysisConfig PhonologicalAnalysis { get; set; }
            = new PhonologicalAnalysisConfig();
    }

    public class AnalysisPipelineFactory
    {
        public AnalysisConfig Config { get; }

        public AnalysisPipelineFactory(AnalysisConfig config)
        {
            Config = config;
        }

        public AnalysisPipeline<WordPhonologyData> Build()
        {
            var modules = new List<IAnalysisModule<WordPhonologyData>>();

            if (Config.PhonologicalAnalysis.Enabled)
                modules.Add(new PhonologicalAnalysisModule(Config.PhonologicalAnalysis));

            return new AnalysisPipeline<WordPhonologyData>(modules.ToArray());
        }
    }
}
