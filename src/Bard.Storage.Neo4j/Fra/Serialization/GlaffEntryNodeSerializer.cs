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
        public MultiNode Serialize(GlaffEntry item)
        {
            var nodeTypes = new List<NodeType>();
            var fields = new List<Field>();

            fields.Add(new Field(PropLabel.GLAFF_RANK, item.Rank));
            fields.Add(new Field(PropLabel.GLAFF_IS_OLD_FASHIONED, item.OldFashioned));
            fields.Add(new Field(PropLabel.GLAFF_GRAPHICAL_FORM, item.GraphicalForm));

            fields.Add(new Field(PropLabel.GLAFF_MORPHO_SYNTAX, item.MorphoSyntax));
            fields.Add(GetField(PropLabel.GLAFF_POS, item.POS));

            if (item.Person.HasValue)
                fields.Add(GetField(PropLabel.GLAFF_PERSON, item.Person.Value));

            if (item.Gender.HasValue)
                fields.Add(GetField(PropLabel.GLAFF_GENDER, item.Gender.Value));

            if (item.Number.HasValue)
                fields.Add(GetField(PropLabel.GLAFF_NUMBER, item.Number.Value));

            if (item.Mood.HasValue)
                fields.Add(GetField(PropLabel.GLAFF_MOOD, item.Mood.Value));

            if (item.Tense.HasValue)
                fields.Add(GetField(PropLabel.GLAFF_TENSE, item.Tense.Value));

            fields.Add(new Field(PropLabel.GLAFF_LEMMA, item.Lemma));
            fields.Add(new Field(PropLabel.GLAFF_IPA_PRONUNCIATIONS, item.IpaPronunciations));
            fields.Add(new Field(PropLabel.GLAFF_SAMPA_PRONUNCIATIONS, item.SampaPronunciations));
            fields.Add(new Field(PropLabel.GLAFF_FRANTEX_ABS_FORM_FREQ, item.FrantexAbsoluteFormFrequency));
            fields.Add(new Field(PropLabel.GLAFF_FRANTEX_REL_FORM_FREQ, item.FrantexRelativeFormFrequency));
            fields.Add(new Field(PropLabel.GLAFF_FRANTEX_ABS_LEMMA_FREQ, item.FrantexAbsoluteLemmaFrequency));
            fields.Add(new Field(PropLabel.GLAFF_FRANTEX_REL_LEMMA_FREQ, item.FrantexRelativeLemmaFrequency));
            fields.Add(new Field(PropLabel.GLAFF_LM10_ABS_FORM_FREQ, item.LM10AbsoluteFormFrequency));
            fields.Add(new Field(PropLabel.GLAFF_LM10_REL_FORM_FREQ, item.LM10RelativeFormFrequency));
            fields.Add(new Field(PropLabel.GLAFF_LM10_ABS_LEMMA_FREQ, item.LM10AbsoluteLemmaFrequency));
            fields.Add(new Field(PropLabel.GLAFF_LM10_REL_LEMMA_FREQ, item.LM10RelativeLemmaFrequency));
            fields.Add(new Field(PropLabel.GLAFF_FRWAC_ABS_FORM_FREQ, item.FrWacAbsoluteFormFrequency));
            fields.Add(new Field(PropLabel.GLAFF_FRWAC_REL_FORM_FREQ, item.FrWacRelativeFormFrequency));
            fields.Add(new Field(PropLabel.GLAFF_FRWAC_ABS_LEMMA_FREQ, item.FrWacAbsoluteLemmaFrequency));
            fields.Add(new Field(PropLabel.GLAFF_FRWAC_REL_LEMMA_FREQ, item.FrWacRelativeLemmaFrequency));


            if (item.MissingPronunciation.HasValue)
                fields.Add(new Field(PropLabel.GLAFF_MISSING_PRONUNCIATION, item.MissingPronunciation.Value));

            if (item.IsAcronym.HasValue)
                fields.Add(new Field(PropLabel.GLAFF_IS_ACRONYM, item.IsAcronym.Value));

            if (item.IsLemma.HasValue)
                fields.Add(new Field(PropLabel.GLAFF_IS_LEMMA, item.IsLemma.Value));


            nodeTypes.Add(new NodeType(
                label: NodeLabel.GLAFF_ENTRY,
                fields: fields.ToArray()));

            return new MultiNode(nodeTypes.ToArray());
        }

        public GlaffEntry Deserialize(INode node)
        {
            EnsureHasLabels(node, NodeLabel.GLAFF_ENTRY);

            var rank = node[PropLabel.GLAFF_RANK].As<int>();
            var oldFashioned = node[PropLabel.GLAFF_IS_OLD_FASHIONED].As<bool>();
            var graphicalForm = node[PropLabel.GLAFF_GRAPHICAL_FORM].As<string>();
            var morphoSyntax = node[PropLabel.GLAFF_MORPHO_SYNTAX].As<string>();
            var pos = Enum.Parse<POS>(node[PropLabel.GLAFF_POS].As<string>());

            Person? person = null;
            if (node.Properties.TryGetValue(PropLabel.GLAFF_PERSON, out var personObj))
                person = Enum.Parse<Person>(personObj.As<string>());

            Gender? gender = null;
            if (node.Properties.TryGetValue(PropLabel.GLAFF_GENDER, out var genderObj))
                gender = Enum.Parse<Gender>(genderObj.As<string>());

            Number? number = null;
            if (node.Properties.TryGetValue(PropLabel.GLAFF_NUMBER, out var numberObj))
                number = Enum.Parse<Number>(numberObj.As<string>());

            Mood? mood = null;
            if (node.Properties.TryGetValue(PropLabel.GLAFF_MOOD, out var moodObj))
                mood = Enum.Parse<Mood>(moodObj.As<string>());

            Tense? tense = null;
            if (node.Properties.TryGetValue(PropLabel.GLAFF_TENSE, out var tenseObj))
                tense = Enum.Parse<Tense>(tenseObj.As<string>());

            var lemma = node[PropLabel.GLAFF_LEMMA].As<string>();
            var ipaPronunciations = node[PropLabel.GLAFF_IPA_PRONUNCIATIONS].As<string>();
            var sampaPronunciations = node[PropLabel.GLAFF_SAMPA_PRONUNCIATIONS].As<string>();
            var frantexAbsFormFreq = node[PropLabel.GLAFF_FRANTEX_ABS_FORM_FREQ].As<int>();
            var frantexRelFormFreq = node[PropLabel.GLAFF_FRANTEX_REL_FORM_FREQ].As<int>();
            var frantexAbsLemmaFreq = node[PropLabel.GLAFF_FRANTEX_ABS_LEMMA_FREQ].As<int>();
            var frantexRelLemmaFreq = node[PropLabel.GLAFF_FRANTEX_REL_LEMMA_FREQ].As<int>();
            var lm10AbsFormFreq = node[PropLabel.GLAFF_LM10_ABS_FORM_FREQ].As<int>();
            var lm10RelFormFreq = node[PropLabel.GLAFF_LM10_REL_FORM_FREQ].As<int>();
            var lm10AbsLemmaFreq = node[PropLabel.GLAFF_LM10_ABS_LEMMA_FREQ].As<int>();
            var lm10RelLemmaFreq = node[PropLabel.GLAFF_LM10_REL_LEMMA_FREQ].As<int>();
            var frwacAbsFormFreq = node[PropLabel.GLAFF_FRWAC_ABS_FORM_FREQ].As<int>();
            var frwacRelFormFreq = node[PropLabel.GLAFF_FRWAC_REL_FORM_FREQ].As<int>();
            var frwacAbsLemmaFreq = node[PropLabel.GLAFF_FRWAC_ABS_LEMMA_FREQ].As<int>();
            var frwacRelLemmaFreq = node[PropLabel.GLAFF_FRWAC_REL_LEMMA_FREQ].As<int>();

            bool? missingPronunciation = null;
            if (node.Properties.TryGetValue(PropLabel.GLAFF_MISSING_PRONUNCIATION, out object missingPronunciationObj))
                missingPronunciation = missingPronunciationObj.As<bool>();

            bool? isAcronym = null;
            if (node.Properties.TryGetValue(PropLabel.GLAFF_IS_ACRONYM, out object isAcronymObj))
                isAcronym = isAcronymObj.As<bool>();

            bool? isLemma = null;
            if (node.Properties.TryGetValue(PropLabel.GLAFF_IS_LEMMA, out object isLemmaObj))
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
