using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.CLI
{
    class Configuration
    {
        public Lexicons Lexicons { get; set; }
    }

    class Lexicons
    {
        public string Main { get; set; }
        public string Oldies { get; set; }
    }
}
