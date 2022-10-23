using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public class WordFormData
    {
        public GlaffEntry[] GlaffEntries { get; }

        public WordFormData(GlaffEntry[] glaffEntries)
        {
            GlaffEntries = glaffEntries;
        }
    }

}
