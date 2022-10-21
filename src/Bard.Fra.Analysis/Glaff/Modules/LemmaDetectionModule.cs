using Bard.Contracts.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Glaff.Modules
{
    public class LemmaDetectionConfig
    {
        public bool Enabled { get; set; } = true;
    }

    public class LemmaDetectionModule : IAnalysisModule<GlaffEntry>
    {
        public LemmaDetectionConfig Config { get; }

        public LemmaDetectionModule(LemmaDetectionConfig config)
        {
            Config = config;
        }

        public bool Analyze(AnalysisResult<GlaffEntry> result)
        {
            bool isLemma = IsLemma(result.Result);

            result.Result.IsLemma = isLemma;

            bool abort = false;
            return abort;
        }

        public bool IsLemma(GlaffEntry entry)
        {
            var pos = entry.POS;
            var number = entry.Number;
            var gender = entry.Gender;
            var mood = entry.Mood;

            if (entry.Lemma != entry.GraphicalForm)
                return false;

            switch (pos)
            {
                case POS.Noun:
                    return number == Number.Singular;
                case POS.Pronoun:
                case POS.Adjective:
                case POS.Determiner:
                    return number == Number.Singular && gender == Gender.Masculine;
                case POS.Verb:
                    return mood == Mood.Infinitive;
                case POS.Adverb:
                case POS.Preposition:
                case POS.Conjunction:
                case POS.Interjection:
                    return true;
                default:
                    throw new NotImplementedException($"Unsupported POS [{pos}].");
            }
        }
    }
}
