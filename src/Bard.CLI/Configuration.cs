using Bard.Fra.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.CLI
{
    class Configuration
    {
        public LexiconConfig Lexicons { get; set; }
        public GraphStorageConfig GraphStorage { get; set; }
        public AnalysisConfig Analysis { get; set; }
    }

    class LexiconConfig
    {
        public string Main { get; set; }
        public string Oldies { get; set; }
        public int Limit { get; set; } = int.MaxValue;
        public int BatchSize { get; set; }
    }

    class GraphStorageConfig
    {
        public string Address { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int BatchSize { get; set; }
    }

    class AnalysisConfig
    {
        public WordFormFilterModuleFactory.Config FilterModule { get; set; }
    }
}
