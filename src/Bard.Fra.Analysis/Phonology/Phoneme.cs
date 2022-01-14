using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Phonology
{
    public static class Phonemes
    {
        public static Phoneme _ = new("");

        public static Phoneme p = new("p", isConsonantic: true, isAnterior: true);
        public static Phoneme t = new("t", isConsonantic: true, isCoronal: true, isAnterior: true);
        public static Phoneme k = new("k", isConsonantic: true, isPosterior: true);
        public static Phoneme b = new("b", isConsonantic: true, isAnterior: true, isVoiced: true);
        public static Phoneme d = new("d", isConsonantic: true, isCoronal: true, isAnterior: true, isVoiced: true);
        public static Phoneme g = new("g", isConsonantic: true, isPosterior: true, isVoiced: true);
        public static Phoneme f = new("f", isConsonantic: true, isAnterior: true, isContinuous: true);
        public static Phoneme s = new("s", isConsonantic: true, isCoronal: true, isAnterior: true, isContinuous: true);
        public static Phoneme S = new("ʃ", isConsonantic: true, isContinuous: true);
        public static Phoneme v = new("v", isConsonantic: true, isAnterior: true, isContinuous: true, isVoiced: true);
        public static Phoneme z = new("z", isConsonantic: true, isCoronal: true, isAnterior: true, isContinuous: true, isVoiced: true);
        public static Phoneme Z = new("ʒ", isConsonantic: true, isContinuous: true, isVoiced: true);
        public static Phoneme m = new("m", isSyllabic: true, isConsonantic: true, isAnterior: true, isNasal: true, isVoiced: true);
        public static Phoneme n = new("n", isSyllabic: true, isConsonantic: true, isCoronal: true, isAnterior: true, isNasal: true, isVoiced: true);
        public static Phoneme nj = new("ɲ", isSyllabic: true, isConsonantic: true, isNasal: true, isVoiced: true);
        public static Phoneme ng = new("ŋ", isSyllabic: true, isConsonantic: true, isPosterior: true, isNasal: true, isVoiced: true);
        public static Phoneme l = new("l", isSyllabic: true, isConsonantic: true, isCoronal: true, isAnterior: true, isContinuous: true, isVoiced: true);
        public static Phoneme R = new("ʁ", isSyllabic: true, isConsonantic: true, isPosterior: true, isContinuous: true, isVoiced: true);
        public static Phoneme X = new("χ", isSyllabic: true, isConsonantic: true, isPosterior: true, isContinuous: true, isVoiced: false);
        public static Phoneme j = new("j", isContinuous: true, isVoiced: true);
        public static Phoneme w = new("w", isPosterior: true, isRounded: true, isContinuous: true, isVoiced: true);
        public static Phoneme Y = new("ɥ", isRounded: true, isContinuous: true, isVoiced: true);

        public static Phoneme i = new("i", isSyllabic: true, isAnterior: true, isHigh: true, isTight: true);
        public static Phoneme e = new("e", isSyllabic: true, isAnterior: true, isTight: true);
        public static Phoneme E = new("ɛ", isSyllabic: true, isAnterior: true);
        public static Phoneme a = new("a", isSyllabic: true, isAnterior: true, isLow: true);
        public static Phoneme y = new("y", isSyllabic: true, isAnterior: true, isRounded: true, isHigh: true, isTight: true);
        public static Phoneme eu = new("ø", isSyllabic: true, isAnterior: true, isRounded: true, isTight: true);
        public static Phoneme oe = new("œ", isSyllabic: true, isAnterior: true, isRounded: true);
        public static Phoneme _e = new("ə", isSyllabic: true, isRounded: true);
        public static Phoneme u = new("u", isSyllabic: true, isPosterior: true, isRounded: true, isHigh: true, isTight: true);
        public static Phoneme o = new("o", isSyllabic: true, isPosterior: true, isRounded: true, isTight: true);
        public static Phoneme O = new("ɔ", isSyllabic: true, isPosterior: true, isRounded: true);
        public static Phoneme A = new("ɑ", isSyllabic: true, isPosterior: true, isRounded: true, isLow: true);

        public static Phoneme @in = new("ɛ̃", isSyllabic: true, isAnterior: true, isNasal: true);
        public static Phoneme an = new("ɑ̃", isSyllabic: true, isAnterior: true, isLow: true, isNasal: true);  // @fixme /a/ ou /ɑ/
        public static Phoneme un = new("œ̃", isSyllabic: true, isAnterior: true, isRounded: true, isNasal: true);
        public static Phoneme on = new("ɔ̃", isSyllabic: true, isPosterior: true, isRounded: true, isNasal: true);

        public static Phoneme[] Vowels = new[]
        {
            i, e, E, a, y, eu, oe, _e, u, o, O, A,
            @in, an, un, on,
        };

        public static Phoneme[] Consonants = new[]
        {
            p, t, k, b, d, g, f, s, S, v, z, Z,
            m, n, nj, ng, l, R, X, j, w, Y,
        };

        public static Phoneme[] ContinuousConsonants = new[]
        {
            f, s, S, v, z, Z, l, R, X,
        };

        public static Phoneme[] All = Vowels.Concat(Consonants).Append(_).ToArray();

        public static Dictionary<string, Phoneme> _phonemesBySymbol = All.ToDictionary(p => p.Symbol);
        public static Phoneme BySymbol(string symbol)
        {
            return _phonemesBySymbol[symbol];
        }
    }

    public static class PhonemeExtensions
    {
        public static string Format(this IEnumerable<Phoneme> phonemes)
        {
            return string.Join(string.Empty, phonemes.Select(p => p.Format()));
        }
    }

    public class Phoneme : IEquatable<Phoneme>
    {
        public string Symbol { get; }

        public bool IsSyllabic { get; }
        public bool IsConsonantic { get; }
        public bool IsCoronal { get; }
        public bool IsAnterior { get; }
        public bool IsPosterior { get; }
        public bool IsRounded { get; }
        public bool IsNasal { get; }
        public bool IsContinuous { get; }
        public bool IsVoiced { get; }
        public bool IsHigh { get; }
        public bool IsLow { get; }
        public bool IsTight { get; }

        public bool IsVowel => IsSyllabic && !IsConsonantic;

        public Phoneme(string symbol, bool isSyllabic = false, bool isConsonantic = false, bool isCoronal = false, bool isAnterior = false, bool isPosterior = false, bool isRounded = false, bool isNasal = false, bool isContinuous = false, bool isVoiced = false, bool isHigh = false, bool isLow = false, bool isTight = false)
        {
            Symbol = symbol;
            IsSyllabic = isSyllabic;
            IsConsonantic = isConsonantic;
            IsCoronal = isCoronal;
            IsAnterior = isAnterior;
            IsPosterior = isPosterior;
            IsRounded = isRounded;
            IsNasal = isNasal;
            IsContinuous = isContinuous;
            IsVoiced = isVoiced;
            IsHigh = isHigh;
            IsLow = isLow;
            IsTight = isTight;
        }

        public string Format() => Symbol;
        public override string ToString()
        {
            return Symbol;
        }

        public override bool Equals(object obj)
        {
            return obj is Phoneme phoneme &&
                   Symbol == phoneme.Symbol &&
                   IsSyllabic == phoneme.IsSyllabic &&
                   IsConsonantic == phoneme.IsConsonantic &&
                   IsCoronal == phoneme.IsCoronal &&
                   IsAnterior == phoneme.IsAnterior &&
                   IsPosterior == phoneme.IsPosterior &&
                   IsRounded == phoneme.IsRounded &&
                   IsNasal == phoneme.IsNasal &&
                   IsContinuous == phoneme.IsContinuous &&
                   IsVoiced == phoneme.IsVoiced &&
                   IsHigh == phoneme.IsHigh &&
                   IsLow == phoneme.IsLow &&
                   IsTight == phoneme.IsTight;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Symbol);
            hash.Add(IsSyllabic);
            hash.Add(IsConsonantic);
            hash.Add(IsCoronal);
            hash.Add(IsAnterior);
            hash.Add(IsPosterior);
            hash.Add(IsRounded);
            hash.Add(IsNasal);
            hash.Add(IsContinuous);
            hash.Add(IsVoiced);
            hash.Add(IsHigh);
            hash.Add(IsLow);
            hash.Add(IsTight);
            return hash.ToHashCode();
        }

        public bool Equals(Phoneme other)
        {
            return Symbol == other.Symbol;
        }
    }

}
