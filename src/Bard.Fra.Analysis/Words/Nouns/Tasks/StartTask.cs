using Bard.Contracts.Fra;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using Bard.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Words.Nouns
{
    public class StartTask
    {
        public Config Config { get; }
        public GraphStorage GraphStorage { get; }

        public StartTask(Config config, GraphStorage graphStorage)
        {
            Config = config;
            GraphStorage = graphStorage;
        }

        public async Task Execute()
        {
            var analysisPipeline = new AnalysisPipelineFactory(Config.Analysis).Build();

            var lemmaData = GraphStorage.GetNounLemmaData();
            var processed = Analyze(analysisPipeline, lemmaData);
            var batches = processed.Batch(100);

            await foreach (var batch in batches)
            {
                await GraphStorage.CreateNounsAsync(batch);
            }
        }

        private async IAsyncEnumerable<LemmaData<NounLemma, NounForm>> Analyze(
            AnalysisPipeline<LemmaData<NounLemma, NounForm>> analysisPipeline,
            IAsyncEnumerable<LemmaData<NounLemma, NounForm>> lemmaData)
        {
            await foreach (var lemma in lemmaData)
            {
                analysisPipeline.Analyze(lemma);
                yield return lemma;
            }
        }
    }
}
