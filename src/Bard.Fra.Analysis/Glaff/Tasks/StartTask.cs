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

namespace Bard.Fra.Analysis.Glaff
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
            var entrySerializer = new GlaffEntryNodeSerializer();
            var pronunSerializer = new PronunciationNodeSerializer();

            var rawEntries = ParseLexicons(Config.Source);

            if (Config.Source.Skip.HasValue)
                rawEntries = rawEntries.Skip(Config.Source.Skip.Value);

            var entryResults = rawEntries.Select(e => analysisPipeline.Analyze(e));

            if (Config.Source.Limit.HasValue)
                entryResults = entryResults.Take(Config.Source.Limit.Value);

            var entries = entryResults.Select(r => r.Result);

            await GraphStorage.CreateGlaffEntriesAsync(entries);
        }

        private static IEnumerable<GlaffEntry> ParseLexicons(SourceConfig config)
        {
            if (!string.IsNullOrWhiteSpace(config.MainDataset))
            {
                var fullPath = Path.GetFullPath(config.MainDataset);
                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"Glàff main lexicon file not found [{fullPath}].");
                    Environment.Exit(1);
                }

                Console.WriteLine($"Loading entries from Glàff main lexicon...");

                int done = 0;
                foreach (var batch in GlaffParser.ParseMainLexicon(fullPath).Batch(config.BatchSize))
                {
                    foreach (var entry in batch)
                        yield return entry;

                    done += batch.Length;

                    if (done % 1000 == 0)
                        Console.WriteLine($"Loaded {done} entries from Glàff main lexicon.");
                }
            }

            if (!string.IsNullOrWhiteSpace(config.OldiesDataset))
            {
                var fullPath = Path.GetFullPath(config.OldiesDataset);
                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"Glàff oldies lexicon file not found [{fullPath}].");
                    Environment.Exit(1);
                }

                Console.WriteLine($"Loading entries from Glàff oldies lexicon...");

                int done = 0;
                foreach (var batch in GlaffParser.ParseOldiesLexicon(fullPath).Batch(config.BatchSize))
                {
                    foreach (var entry in batch)
                        yield return entry;

                    done += batch.Length;

                    if (done % 1000 == 0)
                        Console.WriteLine($"Loaded {done} entries from Glàff oldies lexicon.");
                }
            }
        }

        public class TaskResult
        {
        }
    }
}
