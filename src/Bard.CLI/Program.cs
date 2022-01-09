using Bard.Fra.Analysis;
using Bard.Fra.Glaff;
using Bard.Storage.Neo4j.Fra;
using Bard.Utils;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization.NamingConventions;

namespace Bard.CLI
{
    class Program
    {
        public class Options
        {
            [Value(0, MetaName = "config", HelpText = "Config file")]
            public string ConfigFile { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options opts)
        {
            var config = ParseConfig(opts);
            var graphStorage = InitializeGraphStorage(config.GraphStorage);
            var entries = ParseLexicons(config.Lexicons);
            var pipeline = InitializeAnalysisPipeline(config.Analysis);
            var multiNodeBuilder = new MultiNodeBuilder();

            var wordForms = entries
                .Select(e => pipeline.Analyze(e))
                .Where(w => w.IsValid);

            var multiNodes = wordForms.Select(w => multiNodeBuilder.Build(w)).Take(config.Lexicons.Limit);

            foreach (var batch in multiNodes.Batch(config.GraphStorage.BatchSize))
                AsyncHelpers.RunSync(() => graphStorage.CreateAsync(batch));
        }

        private static Configuration ParseConfig(Options opts)
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var configFullPath = Path.GetFullPath(opts.ConfigFile);
            if (!File.Exists(configFullPath))
            {
                Console.WriteLine($"Config file not found [{configFullPath}].");
                Environment.Exit(1);
            }

            return deserializer.Deserialize<Configuration>(File.ReadAllText(configFullPath));
        }

        private static GraphStorage InitializeGraphStorage(GraphStorageConfig config)
        {
            return new GraphStorage(config.Address, config.User, config.Password);
        }

        private static AnalysisPipeline InitializeAnalysisPipeline(AnalysisPipelineConfig config)
        {
            var modules = new List<IAnalysisModule>();

            if (config.AnomalyDetectorModule.Enabled)
                modules.Add(AnomalyDetectorModuleFactory.Build(config.AnomalyDetectorModule));

            if (config.LemmaDetectionModule.Enabled)
                modules.Add(new LemmaDetectionModule());

            return new AnalysisPipeline(modules.ToArray());
        }

        private static IEnumerable<GlaffEntry> ParseLexicons(LexiconConfig config)
        {
            if (!string.IsNullOrWhiteSpace(config.Main))
            {
                var fullPath = Path.GetFullPath(config.Main);
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

            if (!string.IsNullOrWhiteSpace(config.Oldies))
            {
                var fullPath = Path.GetFullPath(config.Oldies);
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

        static void HandleParseError(IEnumerable<Error> errs)
        {
          //handle errors
        }
    }
}
