using Bard.Storage.Neo4j.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    public class CleanTask
    {
        public GraphStorage GraphStorage { get; }

        public CleanTask(GraphStorage graphStorage)
        {
            GraphStorage = graphStorage;
        }

        public async Task Execute()
        {
            await GraphStorage.DeleteLabel(NodeLabel.PHONETIC_SEQUENCE);
        }

    }
}
