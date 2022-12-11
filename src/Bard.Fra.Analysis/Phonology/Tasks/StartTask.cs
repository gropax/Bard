using Bard.Contracts.Fra.Analysis.Phonology;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using Bard.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
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

            var wordFormData = GraphStorage.GetWordPhonologyData();
            var processed = Analyze(analysisPipeline, wordFormData);
            var batches = processed.Batch(100);

            await foreach (var batch in batches)
            {
                await GraphStorage.CreateWordPhonologyAsync(batch);
            }
        }

        private async IAsyncEnumerable<WordPhonologyData> Analyze(
            AnalysisPipeline<WordPhonologyData> analysisPipeline,
            IAsyncEnumerable<WordPhonologyData> lemmaData)
        {
            await foreach (var lemma in lemmaData)
            {
                analysisPipeline.Analyze(lemma);
                yield return lemma;
            }
        }
    }
}
