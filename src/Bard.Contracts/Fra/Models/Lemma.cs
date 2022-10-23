using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public abstract class Lemma
    {
        public abstract POS POS { get; }
        public string GraphicalForm { get; set; }
    }

    public class NounLemma : Lemma
    {
        public override POS POS => POS.Noun;
        public Gender? Gender { get; set; }
    }
}
