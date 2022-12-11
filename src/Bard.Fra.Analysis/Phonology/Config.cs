using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    public class Config
    {
        public bool Enabled { get; set; } = true;
        public AnalysisConfig Analysis { get; set; } = new AnalysisConfig();
    }
}
