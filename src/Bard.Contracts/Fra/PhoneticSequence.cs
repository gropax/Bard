using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public class PhoneticWord
    {
        public PhoneticSequence PhoneticSequence { get; set; }
        public Rhyme[] Rhymes { get; set; }
        public InnerRhyme[] InnerRhymes { get; set; }
    }

    public class PhoneticSequence
    {
        public long? Id { get; set; }

        public string IpaRepresentation { get; set; }
        public Phoneme[] Phonemes { get; set; }
        public Syllable[] Syllables { get; set; }
    }

    public class PhoneticSequenceIdComparer : IEqualityComparer<PhoneticSequence>
    {
        public bool Equals(PhoneticSequence x, PhoneticSequence y)
        {
            return x.IpaRepresentation == y.IpaRepresentation;
        }

        public int GetHashCode([DisallowNull] PhoneticSequence obj)
        {
            return obj.IpaRepresentation.GetHashCode();
        }
    }

}
