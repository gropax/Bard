using Bard.Fra.Glaff;
using Bard.Utils;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bard.CLI
{
    class Program
    {
        public class Options
        {
            [Value(0, MetaName = "output", HelpText = "Output database file path.")]
            public string DbPath { get; set; }

            [Value(1, MetaName = "lexicon", HelpText = "Glàff main lexicon file path.")]
            public string LexiconPath { get; set; }

            [Value(2, MetaName = "oldies", HelpText = "Glàff oldies lexicon file path.")]
            public string OldiesPath { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        private const int BATCH_SIZE = 50000;

        static void RunOptions(Options opts)
        {
            // init db


            if (!string.IsNullOrWhiteSpace(opts.LexiconPath))
            {
                var fullPath = Path.GetFullPath(opts.LexiconPath);
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

            if (!string.IsNullOrWhiteSpace(opts.OldiesPath))
            {
                var fullPath = Path.GetFullPath(opts.OldiesPath);
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
