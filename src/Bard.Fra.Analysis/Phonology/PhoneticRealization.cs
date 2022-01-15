using Intervals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    public class PhoneticRealization
    {
        public string Graphemes { get; set; }
        public PhoneticSequence PhoneticSequence { get; set; }
        //public Interval<Phoneme[]> Alignment { get; set; }
        public string Alignment { get; set; }
        public bool IsStandard { get; set; }
    }
}
