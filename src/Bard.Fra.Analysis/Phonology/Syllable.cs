using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    public static class SyllableExtensions
    {
        public static string Format(this Syllable[] syllables)
        {
            return string.Join(".", syllables.Select(s => s.Format()));
        }
    }

    public class Syllable
    {
        public static Syllable Null { get; } = new Syllable(Phonology.Phonemes._);

        public Phoneme[] Onset { get; }
        public Phoneme Nucleus { get; }
        public Phoneme[] Coda { get; }
        public IEnumerable<Phoneme> Phonemes => Onset.Append(Nucleus).Concat(Coda);

        public bool IsOpen => Coda.Length == 0;
        public bool IsClose => Coda.Length > 0;
        public Phoneme[] Rhyme => Coda.Prepend(Nucleus).ToArray();
        public Syllable RhymeSyllable => new Syllable(null, Nucleus, Coda);

        public Syllable(Phoneme nucleus) : this(null, nucleus, null) { }
        public Syllable(Phoneme[] onset, Phoneme nucleus) : this(onset, nucleus, null) { }
        public Syllable(Phoneme nucleus, Phoneme[] coda) : this(null, nucleus, coda) { }
        public Syllable(Phoneme[] onset, Phoneme nucleus, Phoneme[] coda)
        {
            Onset = onset ?? Array.Empty<Phoneme>();
            Nucleus = nucleus;
            Coda = coda ?? Array.Empty<Phoneme>();
        }

        public string Format() => string.Join("", Phonemes.Select(p => p.Symbol));
        public override string ToString()
        {
            return string.Join("", Phonemes.Select(p => p.Symbol));
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
