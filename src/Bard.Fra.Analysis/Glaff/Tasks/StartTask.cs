﻿using Bard.Storage.Neo4j.Fra;
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

            var entryNodes = ParseLexicons(Config.Source)
                .Select(e => analysisPipeline.Analyze(e))
                .Select(e => entrySerializer.Serialize(e.Result));
                //.Where(w => w.IsValid)

            if (Config.Source.Limit.HasValue)
                entryNodes = entryNodes.Take(Config.Source.Limit.Value);

            foreach (var batch in entryNodes.Batch(1000))
            {
                await GraphStorage.CreateAsync(batch);
            }
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