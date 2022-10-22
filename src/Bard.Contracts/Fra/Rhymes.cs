using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public class Rhyme
    {
        public PhoneticSequence PhoneticSequence { get; set; }
    }

    public class InnerRhyme
    {
        public PhoneticSequence PhoneticSequence { get; set; }
        public int SyllableNumber { get; set; }
    }

    public class Rhymes
    {
        public Phoneme[] FinalRhyme { get; set; }
        public Phoneme[][] InnerRhymes { get; set; }
    }
}
