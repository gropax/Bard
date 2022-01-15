using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    public class PhoneticWord
    {
        public PhoneticSequence PhoneticSequence { get; set; }
        public Rhyme[] Rhymes { get; set; }
        public InnerRhyme[] InnerRhymes { get; set; }
    }

    public class PhoneticSequence
    {
        public string Id { get; set; }
        public Phoneme[] Phonemes { get; set; }
        public Syllable[] Syllables { get; set; }
    }

    public class PhoneticSequenceIdComparer : IEqualityComparer<PhoneticSequence>
    {
        public bool Equals(PhoneticSequence x, PhoneticSequence y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] PhoneticSequence obj)
        {
            return obj.Id.GetHashCode();
        }
    }

}
