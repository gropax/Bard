using Bard.Fra.Analysis;
using Bard.Fra.Analysis.Glaff;
using Bard.Storage.Neo4j.Fra;
using Bard.Utils;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;

namespace Bard.CLI
{
    class Program
    {
        [Verb("init", HelpText = "Initialize the graph storage.")]
        class InitOptions
        {
            [Option('c', "config", Required = true, HelpText = "Config file")]
            public string ConfigFile { get; set; }
        }

        [Verb("start", HelpText = "Start tasks.")]
        class StartOptions
        {
            [Value(0, MetaName = "tasks", HelpText = "Comma separated list of tasks.")]
            public string Tasks { get; set; }

            [Option('c', "config", Required = true, HelpText = "Config file")]
            public string ConfigFile { get; set; }
        }

        [Verb("clean", HelpText = "Remove previous task results.")]
        class CleanOptions
        {
            [Value(0, MetaName = "tasks", HelpText = "Comma separated list of tasks.")]
            public string Tasks { get; set; }

            [Option('c', "config", Required = true, HelpText = "Config file")]
            public string ConfigFile { get; set; }
        }

        enum TaskId
        {
            Glaff,
            Words,
                Nouns,
            Phonology,
        }

        static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<InitOptions, StartOptions, CleanOptions>(args)
                .WithParsed<InitOptions>(options => InitCommand(options))
                .WithParsed<StartOptions>(options => StartCommand(options))
                .WithParsed<CleanOptions>(options => CleanCommand(options))
                .WithNotParsed(HandleParseError);
        }

        static void InitCommand(InitOptions opts)
        {
            var config = ParseConfig(opts.ConfigFile);
            var graphStorage = GetGraphStorage(config.GraphStorage);
        }

        static void StartCommand(StartOptions opts)
        {
            AsyncHelpers.RunSync(() => StartCommandAsync(opts));
        }

        static async Task StartCommandAsync(StartOptions opts)
        {
            var config = ParseConfig(opts.ConfigFile);
            var tasks = ParseTaskIds(opts.Tasks);

            var graphStorage = GetGraphStorage(config.GraphStorage);

            if (tasks.Contains(TaskId.Glaff))
            {
                await new CleanTask(config.DataSources.Glaff, graphStorage).Execute();
                await new StartTask(config.DataSources.Glaff, graphStorage).Execute();
            }

            if (tasks.Contains(TaskId.Nouns) || tasks.Contains(TaskId.Words))
            {
                await new Fra.Analysis.Words.Nouns.CleanTask(graphStorage).Execute();
                await new Fra.Analysis.Words.Nouns.StartTask(config.Analysis.Words.Nouns, graphStorage).Execute();
            }

            if (tasks.Contains(TaskId.Phonology))
            {
                await new Fra.Analysis.Phonology.CleanTask(graphStorage).Execute();
                await new Fra.Analysis.Phonology.StartTask(config.Analysis.Phonology, graphStorage).Execute();
            }
        }

        static void CleanCommand(CleanOptions opts)
        {
            AsyncHelpers.RunSync(() => CleanCommandAsync(opts));
        }

        static async Task CleanCommandAsync(CleanOptions opts)
        {
            var config = ParseConfig(opts.ConfigFile);
            var tasks = ParseTaskIds(opts.Tasks);

            var graphStorage = GetGraphStorage(config.GraphStorage);

            if (tasks.Contains(TaskId.Glaff))
                await new CleanTask(config.DataSources.Glaff, graphStorage).Execute();

            if (tasks.Contains(TaskId.Nouns) || tasks.Contains(TaskId.Words))
                await new Fra.Analysis.Words.Nouns.CleanTask(graphStorage).Execute();
        }

        static List<TaskId> ParseTaskIds(string tasks)
        {
            var taskList = new List<TaskId>();

            foreach (var taskStr in tasks.Split(','))
            {
                if (!Enum.TryParse<TaskId>(taskStr, out var taskId))
                    throw new Exception($"Unknown task [{taskStr}].");
                else
                    taskList.Add(taskId);
            }

            return taskList;
        }

        private static Configuration ParseConfig(string configFile)
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var configFullPath = Path.GetFullPath(configFile);
            if (!File.Exists(configFullPath))
            {
                Console.WriteLine($"Config file not found [{configFullPath}].");
                Environment.Exit(1);
            }

            return deserializer.Deserialize<Configuration>(File.ReadAllText(configFullPath));
        }

        private static GraphStorage GetGraphStorage(GraphStorageConfig config)
        {
            return new GraphStorage(config.Address, config.User, config.Password);
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
        }
    }
}
