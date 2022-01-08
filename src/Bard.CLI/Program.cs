using Bard.Fra.Glaff;
using Bard.Utils;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
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

        private const int BATCH_SIZE = 10000;

        static void RunOptions(Options opts)
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

            var config = deserializer.Deserialize<Configuration>(File.ReadAllText(configFullPath));

            // init db


            if (!string.IsNullOrWhiteSpace(config.Lexicons.Main))
            {
                var fullPath = Path.GetFullPath(config.Lexicons.Main);
                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"Glàff main lexicon file not found [{fullPath}].");
                    Environment.Exit(1);
                }

                Console.WriteLine($"Loading entries from Glàff main lexicon...");

                int done = 0;
                foreach (var batch in GlaffParser.ParseMainLexicon(fullPath).Batch(BATCH_SIZE))
                {
                    // create entries in db

                    done += batch.Length;

                    if (done % 1000 == 0)
                        Console.WriteLine($"Loaded {done} entries from Glàff main lexicon.");
                }
            }

            if (!string.IsNullOrWhiteSpace(config.Lexicons.Oldies))
            {
                var fullPath = Path.GetFullPath(config.Lexicons.Oldies);
                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"Glàff oldies lexicon file not found [{fullPath}].");
                    Environment.Exit(1);
                }

                Console.WriteLine($"Loading entries from Glàff oldies lexicon...");

                int done = 0;
                foreach (var batch in GlaffParser.ParseOldiesLexicon(fullPath).Batch(BATCH_SIZE))
                {
                    // create entries in db

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
