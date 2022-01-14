using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    public class Syllable
    {
        public static Syllable Null { get; } = new Syllable(Phonology.Phonemes._);

        public Phoneme[] Onset { get; }
        public Phoneme Nucleus { get; }
        public Phoneme[] Coda { get; }
        public IEnumerable<Phoneme> Phonemes => Onset.Append(Nucleus).Concat(Coda);

        public Syllable(Phoneme nucleus) : this(null, nucleus, null) { }
        public Syllable(Phoneme[] onset, Phoneme nucleus) : this(onset, nucleus, null) { }
        public Syllable(Phoneme nucleus, Phoneme[] coda) : this(null, nucleus, coda) { }
        public Syllable(Phoneme[] onset, Phoneme nucleus, Phoneme[] coda)
        {
            Onset = onset ?? Array.Empty<Phoneme>();
            Nucleus = nucleus;
            Coda = coda ?? Array.Empty<Phoneme>();
        }

        public override string ToString()
        {
            var symbols = string.Join("", Phonemes.Select(p => p.Symbol));
            return $"/{symbols}/";
        }

        public override bool Equals(object obj)
        {
            return obj is Syllable syllable &&
                   Enumerable.SequenceEqual(Onset, syllable.Onset) &&
                   EqualityComparer<Phoneme>.Default.Equals(Nucleus, syllable.Nucleus) &&
                   Enumerable.SequenceEqual(Coda, syllable.Coda);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Onset, Nucleus, Coda, Phonemes);
        }
    }


}
