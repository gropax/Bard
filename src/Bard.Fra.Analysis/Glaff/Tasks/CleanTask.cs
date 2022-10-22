using Bard.Storage.Neo4j.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Glaff
{
    public class CleanTask
    {
        public Config Config { get; }
        public GraphStorage GraphStorage { get; }

        public CleanTask(Config config, GraphStorage graphStorage)
        {
            Config = config;
            GraphStorage = graphStorage;
        }

        public async Task Execute()
        {
            await GraphStorage.DeleteGlaffContent();
        }

    }
}
