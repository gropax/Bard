using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public class PhoneticRealization
    {
        public string Graphemes { get; set; }
        public PhoneticWord PhoneticWord { get; set; }
        //public Interval<Phoneme[]> Alignment { get; set; }
        public string Alignment { get; set; }
        public bool IsStandard { get; set; }
    }
}
