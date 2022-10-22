using Bard.Contracts.Fra;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public class GlaffEntryNodeSerializer : INodeSerializer<GlaffEntry>
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
                label: NodeLabel.LABEL_GLAFF_ENTRY,
                fields: fields.ToArray()));

            return new MultiNode(nodeTypes.ToArray());
        }

        public GlaffEntry Deserialize(MultiNode node)
        {
            throw new NotImplementedException();
        }

        protected Field GetField<T>(string fieldName, T value) where T : Enum
        {
            return new Field(fieldName, value.ToString());
        }
    }
}
