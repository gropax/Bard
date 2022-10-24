using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public interface ILemma
    {
        long? Id { get; }
        abstract POS POS { get; }
        string GraphicalForm { get; }
    }

    public abstract class Lemma : ILemma
    {
        public long? Id { get; set; }

        public abstract POS POS { get; }
        public string GraphicalForm { get; set; }
    }

    public class NounLemma : Lemma
    {
        public override POS POS => POS.Noun;
        public Gender? Gender { get; set; }
    }
}
