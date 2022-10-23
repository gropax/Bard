using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public class LemmaData
    {
        public WordFormData[] WordForms { get; }

        public LemmaData(WordFormData[] wordForms)
        {
            WordForms = wordForms;
        }
    }

}
