using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    public class Rhyme
    {
        public Phoneme[] Phonemes { get; set; }
        public bool IsFinal { get; set; }
        public int SyllableNumber { get; set; }
    }

    public class Rhymes
    {
        public Phoneme[] FinalRhyme { get; set; }
        public Phoneme[][] InnerRhymes { get; set; }
    }
}
