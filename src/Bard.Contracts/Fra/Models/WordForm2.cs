using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public abstract class WordForm2
    {
        public abstract POS POS { get; }
        public string Lemma { get; set; }
        public string GraphicalForm { get; set; }
    }

    public class NounForm : WordForm2
    {
        public override POS POS => POS.Noun;
        public Gender? Gender { get; set; }
        public Number? Number { get; set; }
    }

}
