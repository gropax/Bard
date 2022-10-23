using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public class Pronunciation
    {
        public string Graphemes { get; set; }
        public string Value { get; set; }
        public string[] Phonemes { get; set; }
        public string Alignment { get; set; }
        public ChangeHistory History { get; }
        public List<IAnomaly> Anomalies { get; } = new List<IAnomaly>();

        public bool IsValid { get; set; } = true;
        public bool? AlignmentFailed { get; set; }
        public bool? BadSyllabation { get; set; }

        public Pronunciation() { }
        public Pronunciation(string graphemes, string value)
        {
            Graphemes = graphemes;
            Value = value;
            History = new ChangeHistory(value);
        }
    }

}
