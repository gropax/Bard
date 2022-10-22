using Bard.Fra.Analysis;
using Bard.Fra.Analysis.Glaff.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Glaff
{
    public class AnalysisPipelineFactory
    {
        public AnalysisConfig Config { get; }

        public AnalysisPipelineFactory(AnalysisConfig config)
        {
            Config = config;
        }

        public AnalysisPipeline<GlaffEntry> Build()
        {
            var modules = new List<IAnalysisModule<GlaffEntry>>();

            if (Config.MissingPronunciationDetection.Enabled)
                modules.Add(new MissingPronunciationDetectionModule(Config.MissingPronunciationDetection));

            if (Config.AcronymDetection.Enabled)
                modules.Add(new AcronymDetectionModule(Config.AcronymDetection));

            if (Config.LemmaDetection.Enabled)
                modules.Add(new Modules.LemmaDetectionModule(Config.LemmaDetection));

            if (Config.PronunciationCleaning.Enabled)
                modules.Add(new Modules.PronunciationCleaningModuleFactory(Config.PronunciationCleaning).Build());

            return new AnalysisPipeline<GlaffEntry>(modules.ToArray());
        }
    }
}
