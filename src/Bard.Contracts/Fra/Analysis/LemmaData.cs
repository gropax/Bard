using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra.Analysis.Words
{
    public class LemmaData<TLemma, TWordForm>
        where TLemma : ILemma
        where TWordForm : IWordForm
    {
        public WordFormData<TWordForm>[] WordForms { get; }
        public TLemma Lemma { get; set; }

        public LemmaData(WordFormData<TWordForm>[] wordForms)
        {
            WordForms = wordForms;
        }
    }

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
