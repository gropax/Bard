using Bard.Contracts.Fra;
using Bard.Fra.Analysis;
using Bard.Fra.Analysis.Glaff.Modules;
using Bard.Fra.Analysis.Words.Nouns.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Words.Nouns
{
    public class AnalysisConfig
    {
        public WordBuildingConfig WordBuilding { get; set; }
            = new WordBuildingConfig();
    }

    public class AnalysisPipelineFactory
    {
        public AnalysisConfig Config { get; }

        public AnalysisPipelineFactory(AnalysisConfig config)
        {
            Config = config;
        }

        public AnalysisPipeline<LemmaData<NounLemma, NounForm>> Build()
        {
            var modules = new List<IAnalysisModule<LemmaData<NounLemma, NounForm>>>();

            if (Config.WordBuilding.Enabled)
                modules.Add(new WordBuildingModule(Config.WordBuilding));

            return new AnalysisPipeline<LemmaData<NounLemma, NounForm>>(modules.ToArray());
        }
    }
}
