using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public class WordFormData<TWordForm> where TWordForm : IWordForm
    {
        public GlaffEntry[] GlaffEntries { get; }

        public TWordForm WordForm { get; set; }
        public Pronunciation[] Pronunciations { get; set; }

        public WordFormData(GlaffEntry[] glaffEntries)
        {
            GlaffEntries = glaffEntries;
        }
    }

}
