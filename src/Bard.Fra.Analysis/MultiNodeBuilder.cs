using Bard.Contracts.Fra;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public class MultiNodeBuilder
    {
        public const string LABEL_WORD_FORM = "WordForm";
        public const string LABEL_LEMMA = "Lemma";

        public MultiNode Build(WordForm wordForm)
        {
            var nodeTypes = new List<NodeType>();

            nodeTypes.Add(new NodeType(
                label: LABEL_WORD_FORM,
                fields: GetWordFormNodeFields(wordForm)));

            if (wordForm.IsLemma)
                nodeTypes.Add(new NodeType(label: LABEL_LEMMA));

            return new MultiNode(nodeTypes.ToArray());
        }

        private Field[] GetWordFormNodeFields(WordForm wordForm)
        {
            var fields = new List<Field>();
            var entry = wordForm.GlaffEntry;

            fields.Add(new Field(Format(WordFormField.Graphemes), entry.GraphicalForm));
            fields.Add(new Field(Format(WordFormField.Phonemes), entry.ApiPronunciations));
            fields.Add(new Field(Format(WordFormField.Lemma), entry.Lemma));

            if (entry.Number.HasValue)
                fields.Add(new Field(Format(WordFormField.Number), Format(entry.Number.Value)));

            if (entry.Gender.HasValue)
                fields.Add(new Field(Format(WordFormField.Gender), Format(entry.Gender.Value)));

            if (entry.Person.HasValue)
                fields.Add(new Field(Format(WordFormField.Person), Format(entry.Person.Value)));

            if (entry.Mood.HasValue)
                fields.Add(new Field(Format(WordFormField.Mood), Format(entry.Mood.Value)));

            return fields.ToArray();
        }


        private string Format(WordFormField field)
        {
            switch (field)
            {
                case WordFormField.Graphemes: return "graphemes";
                case WordFormField.Phonemes: return "phonemes";
                case WordFormField.Lemma: return "lemma";
                case WordFormField.POS: return "pos";
                case WordFormField.Number: return "number";
                case WordFormField.Gender: return "gender";
                case WordFormField.Person: return "person";
                case WordFormField.Mood: return "mood";
                default:
                    throw new NotImplementedException($"Unsupported word form field [{field}].");
            }
        }

        private string Format(Number number)
        {
            switch (number)
            {
                case Number.Singular: return "sg";
                case Number.Plural: return "pl";
                default:
                    throw new NotImplementedException($"Unsupported number [{number}].");
            }
        }

        private string Format(Gender gender)
        {
            switch (gender)
            {
                case Gender.Masculine: return "m";
                case Gender.Feminine: return "f";
                default:
                    throw new NotImplementedException($"Unsupported gender [{gender}].");
            }
        }

        private string Format(Person person)
        {
            switch (person)
            {
                case Person.Indeterminate: return "i";
                case Person.First: return "1";
                case Person.Second: return "2";
                case Person.Third: return "3";
                default:
                    throw new NotImplementedException($"Unsupported person [{person}].");
            }
        }

        private string Format(Mood mood)
        {
            switch (mood)
            {
                case Mood.Infinitive: return "inf";
                case Mood.PastParticiple: return "part";
                case Mood.Indicative: return "ind";
                case Mood.Subjunctive: return "subj";
                case Mood.Conditional: return "cond";
                case Mood.Imperative: return "imp";
                default:
                    throw new NotImplementedException($"Unsupported mood [{mood}].");
            }
        }
    }
}
