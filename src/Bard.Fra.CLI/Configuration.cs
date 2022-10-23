using Bard.Fra.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.CLI
{
    public class Configuration
    {
        public GraphStorageConfig GraphStorage { get; set; }
        public DataSourceConfig DataSources { get; set; }
        public AnalysisConfig Analysis { get; set; }
    }

    public class GraphStorageConfig
    {
        public string Address { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int BatchSize { get; set; }
    }

    public class DataSourceConfig
    {
        public Fra.Analysis.Glaff.Config Glaff { get; set; }
    }

    public class AnalysisConfig
    {
        public Fra.Analysis.Words.Config Words { get; set; }
    }
}
