using Bard.Contracts.Fra;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public class GlaffEntryNodeSerializer : NodeSerializerBase, INodeSerializer<GlaffEntry>
    {
        public const string RANK = "glaff.raw.rank";
        public const string IS_OLD_FASHIONED = "glaff.raw.is_old_fashioned";
        public const string GRAPHICAL_FORM = "glaff.raw.graphical_form";
        public const string MORPHO_SYNTAX = "glaff.raw.morpho_syntax";
        public const string POS = "glaff.raw.pos";
        public const string PERSON = "glaff.raw.person";
        public const string GENDER = "glaff.raw.gender";
        public const string NUMBER = "glaff.raw.number";
        public const string MOOD = "glaff.raw.mood";
        public const string TENSE = "glaff.raw.tense";
        public const string LEMMA = "glaff.raw.lemma";
        public const string IPA_PRONUNCIATIONS = "glaff.raw.ipa_pronunciations";
        public const string SAMPA_PRONUNCIATIONS = "glaff.raw.sampa_pronunciations";
        public const string FRANTEX_ABS_FORM_FREQ = "glaff.raw.frantex_abs_form_freq";
        public const string FRANTEX_REL_FORM_FREQ = "glaff.raw.frantex_rel_form_freq";
        public const string FRANTEX_ABS_LEMMA_FREQ = "glaff.raw.frantex_abs_lemma_freq";
        public const string FRANTEX_REL_LEMMA_FREQ = "glaff.raw.frantex_rel_lemma_freq";
        public const string LM10_ABS_FORM_FREQ = "glaff.raw.lm10_abs_form_freq";
        public const string LM10_REL_FORM_FREQ = "glaff.raw.lm10_rel_form_freq";
        public const string LM10_ABS_LEMMA_FREQ = "glaff.raw.lm10_abs_lemma_freq";
        public const string LM10_REL_LEMMA_FREQ = "glaff.raw.lm10_rel_lemma_freq";
        public const string FRWAC_ABS_FORM_FREQ = "glaff.raw.frwac_abs_form_freq";
        public const string FRWAC_REL_FORM_FREQ = "glaff.raw.frwac_rel_form_freq";
        public const string FRWAC_ABS_LEMMA_FREQ = "glaff.raw.frwac_abs_lemma_freq";
        public const string FRWAC_REL_LEMMA_FREQ = "glaff.raw.frwac_rel_lemma_freq";
        public const string MISSING_PRONUNCIATION = "glaff.calc.missing_pronunciation";
        public const string IS_ACRONYM = "glaff.calc.is_acronym";
        public const string IS_LEMMA = "glaff.calc.is_lemma";

        public MultiNode Serialize(GlaffEntry item)
        {
            var nodeTypes = new List<NodeType>();
            var fields = new List<Field>();

            fields.Add(new Field(RANK, item.Rank));
            fields.Add(new Field(IS_OLD_FASHIONED, item.OldFashioned));
            fields.Add(new Field(GRAPHICAL_FORM, item.GraphicalForm));

            fields.Add(new Field(MORPHO_SYNTAX, item.MorphoSyntax));
            fields.Add(GetField(POS, item.POS));

            if (item.Person.HasValue)
                fields.Add(GetField(PERSON, item.Person.Value));

            if (item.Gender.HasValue)
                fields.Add(GetField(GENDER, item.Gender.Value));

            if (item.Number.HasValue)
                fields.Add(GetField(NUMBER, item.Number.Value));

            if (item.Mood.HasValue)
                fields.Add(GetField(MOOD, item.Mood.Value));

            if (item.Tense.HasValue)
                fields.Add(GetField(TENSE, item.Tense.Value));

            fields.Add(new Field(LEMMA, item.Lemma));
            fields.Add(new Field(IPA_PRONUNCIATIONS, item.IpaPronunciations));
            fields.Add(new Field(SAMPA_PRONUNCIATIONS, item.SampaPronunciations));
            fields.Add(new Field(FRANTEX_ABS_FORM_FREQ, item.FrantexAbsoluteFormFrequency));
            fields.Add(new Field(FRANTEX_REL_FORM_FREQ, item.FrantexRelativeFormFrequency));
            fields.Add(new Field(FRANTEX_ABS_LEMMA_FREQ, item.FrantexAbsoluteLemmaFrequency));
            fields.Add(new Field(FRANTEX_REL_LEMMA_FREQ, item.FrantexRelativeLemmaFrequency));
            fields.Add(new Field(LM10_ABS_FORM_FREQ, item.LM10AbsoluteFormFrequency));
            fields.Add(new Field(LM10_REL_FORM_FREQ, item.LM10RelativeFormFrequency));
            fields.Add(new Field(LM10_ABS_LEMMA_FREQ, item.LM10AbsoluteLemmaFrequency));
            fields.Add(new Field(LM10_REL_LEMMA_FREQ, item.LM10RelativeLemmaFrequency));
            fields.Add(new Field(FRWAC_ABS_FORM_FREQ, item.FrWacAbsoluteFormFrequency));
            fields.Add(new Field(FRWAC_REL_FORM_FREQ, item.FrWacRelativeFormFrequency));
            fields.Add(new Field(FRWAC_ABS_LEMMA_FREQ, item.FrWacAbsoluteLemmaFrequency));
            fields.Add(new Field(FRWAC_REL_LEMMA_FREQ, item.FrWacRelativeLemmaFrequency));


            if (item.MissingPronunciation.HasValue)
                fields.Add(new Field(MISSING_PRONUNCIATION, item.MissingPronunciation.Value));

            if (item.IsAcronym.HasValue)
                fields.Add(new Field(IS_ACRONYM, item.IsAcronym.Value));

            if (item.IsLemma.HasValue)
                fields.Add(new Field(IS_LEMMA, item.IsLemma.Value));


            nodeTypes.Add(new NodeType(
                label: NodeLabel.GLAFF_ENTRY,
                fields: fields.ToArray()));

            return new MultiNode(nodeTypes.ToArray());
        }

        public GlaffEntry Deserialize(INode node)
        {
            EnsureHasLabels(node, NodeLabel.GLAFF_ENTRY);

            var rank = node[RANK].As<int>();
            var oldFashioned = node[IS_OLD_FASHIONED].As<bool>();
            var graphicalForm = node[GRAPHICAL_FORM].As<string>();
            var morphoSyntax = node[MORPHO_SYNTAX].As<string>();
            var pos = Enum.Parse<POS>(node[POS].As<string>());

            Person? person = null;
            if (node.Properties.TryGetValue(PERSON, out var personObj))
                person = Enum.Parse<Person>(personObj.As<string>());

            Gender? gender = null;
            if (node.Properties.TryGetValue(GENDER, out var genderObj))
                gender = Enum.Parse<Gender>(genderObj.As<string>());

            Number? number = null;
            if (node.Properties.TryGetValue(NUMBER, out var numberObj))
                number = Enum.Parse<Number>(numberObj.As<string>());

            Mood? mood = null;
            if (node.Properties.TryGetValue(MOOD, out var moodObj))
                mood = Enum.Parse<Mood>(moodObj.As<string>());

            Tense? tense = null;
            if (node.Properties.TryGetValue(TENSE, out var tenseObj))
                tense = Enum.Parse<Tense>(tenseObj.As<string>());

            var lemma = node[LEMMA].As<string>();
            var ipaPronunciations = node[IPA_PRONUNCIATIONS].As<string>();
            var sampaPronunciations = node[SAMPA_PRONUNCIATIONS].As<string>();
            var frantexAbsFormFreq = node[FRANTEX_ABS_FORM_FREQ].As<int>();
            var frantexRelFormFreq = node[FRANTEX_REL_FORM_FREQ].As<int>();
            var frantexAbsLemmaFreq = node[FRANTEX_ABS_LEMMA_FREQ].As<int>();
            var frantexRelLemmaFreq = node[FRANTEX_REL_LEMMA_FREQ].As<int>();
            var lm10AbsFormFreq = node[LM10_ABS_FORM_FREQ].As<int>();
            var lm10RelFormFreq = node[LM10_REL_FORM_FREQ].As<int>();
            var lm10AbsLemmaFreq = node[LM10_ABS_LEMMA_FREQ].As<int>();
            var lm10RelLemmaFreq = node[LM10_REL_LEMMA_FREQ].As<int>();
            var frwacAbsFormFreq = node[FRWAC_ABS_FORM_FREQ].As<int>();
            var frwacRelFormFreq = node[FRWAC_REL_FORM_FREQ].As<int>();
            var frwacAbsLemmaFreq = node[FRWAC_ABS_LEMMA_FREQ].As<int>();
            var frwacRelLemmaFreq = node[FRWAC_REL_LEMMA_FREQ].As<int>();

            bool? missingPronunciation = null;
            if (node.Properties.TryGetValue(MISSING_PRONUNCIATION, out object missingPronunciationObj))
                missingPronunciation = missingPronunciationObj.As<bool>();

            bool? isAcronym = null;
            if (node.Properties.TryGetValue(IS_ACRONYM, out object isAcronymObj))
                isAcronym = isAcronymObj.As<bool>();

            bool? isLemma = null;
            if (node.Properties.TryGetValue(IS_LEMMA, out object isLemmaObj))
                isLemma = isLemmaObj.As<bool>();

            return new GlaffEntry()
            {
                Id = node.Id,
                Rank = rank,
                OldFashioned = oldFashioned,
                GraphicalForm = graphicalForm,
                MorphoSyntax = morphoSyntax,
                POS = pos,
                Person = person,
                Gender = gender,
                Number = number,
                Mood = mood,
                Tense = tense,
                Lemma = lemma,
                IpaPronunciations = ipaPronunciations,
                SampaPronunciations = sampaPronunciations,
                FrantexAbsoluteFormFrequency = frantexAbsFormFreq,
                FrantexRelativeFormFrequency = frantexRelFormFreq,
                FrantexAbsoluteLemmaFrequency = frantexAbsLemmaFreq,
                FrantexRelativeLemmaFrequency = frantexRelLemmaFreq,
                LM10AbsoluteFormFrequency = lm10AbsFormFreq,
                LM10RelativeFormFrequency = lm10RelFormFreq,
                LM10AbsoluteLemmaFrequency = lm10AbsLemmaFreq,
                LM10RelativeLemmaFrequency = lm10RelLemmaFreq,
                FrWacAbsoluteFormFrequency = frwacAbsFormFreq,
                FrWacRelativeFormFrequency = frwacRelFormFreq,
                FrWacAbsoluteLemmaFrequency = frwacAbsLemmaFreq,
                FrWacRelativeLemmaFrequency = frwacRelLemmaFreq,
                MissingPronunciation = missingPronunciation,
                IsAcronym = isAcronym,
                IsLemma = isLemma,
            };
        }
    }
}
