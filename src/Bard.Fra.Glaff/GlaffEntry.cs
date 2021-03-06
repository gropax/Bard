using Bard.Contracts.Fra;
using System;

namespace Bard.Fra.Glaff
{
    public class GlaffEntry
    {
        public int Rank { get; set; }
        public bool OldFashioned { get; set; }
        public string GraphicalForm { get; set; }

        public string MorphoSyntax { get; set; }
        public POS POS { get; set; }
        public Person? Person { get; set; }
        public Gender? Gender { get; set; }
        public Number? Number { get; set; }
        public Mood? Mood { get; set; }
        public Tense? Tense { get; set; }

        public string Lemma { get; set; }
        public string IpaPronunciations { get; set; }
        public string SampaPronunciations { get; set; }
        public double FrantexAbsoluteFormFrequency { get; set; }
        public double FrantexRelativeFormFrequency { get; set; }
        public double FrantexAbsoluteLemmaFrequency { get; set; }
        public double FrantexRelativeLemmaFrequency { get; set; }
        public double LM10AbsoluteFormFrequency { get; set; }
        public double LM10RelativeFormFrequency { get; set; }
        public double LM10AbsoluteLemmaFrequency { get; set; }
        public double LM10RelativeLemmaFrequency { get; set; }
        public double FrWacAbsoluteFormFrequency { get; set; }
        public double FrWacRelativeFormFrequency { get; set; }
        public double FrWacAbsoluteLemmaFrequency { get; set; }
        public double FrWacRelativeLemmaFrequency { get; set; }
    }


}
