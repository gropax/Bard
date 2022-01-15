using Bard.Contracts.Fra;
using Bard.Fra.Analysis.Phonology;
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
        public const string LABEL_PHON_SEQ = "PhonSeq";
        public const string LABEL_LEMMA = "Lemma";

        public Relationship Build(long originId, long targetId, PhoneticRealization realization)
        {
            var fields = new List<Field>();

            fields.Add(new Field(Format(PhoneticRealizationField.IsStandard), realization.IsStandard));

            return new Relationship(
                originId, targetId,
                Format(RelationshipType.PhoneticRealization),
                fields.ToArray());
        }

        public Relationship Build(long originId, long targetId, Rhyme rhyme)
        {
            var fields = new List<Field>();

            return new Relationship(
                originId, targetId,
                Format(RelationshipType.Rhyme),
                fields.ToArray());
        }

        public Relationship Build(long originId, long targetId, InnerRhyme rhyme)
        {
            var fields = new List<Field>();

            fields.Add(new Field(Format(InnerRhymeField.SyllableNumber), rhyme.SyllableNumber));

            return new Relationship(
                originId, targetId,
                Format(RelationshipType.InnerRhyme),
                fields.ToArray());
        }

        public MultiNode Build(PhoneticSequence phonSeq)
        {
            var nodeTypes = new List<NodeType>();

            nodeTypes.Add(new NodeType(
                label: LABEL_PHON_SEQ,
                fields: GetPhoneticSequenceNodeFields(phonSeq)));

            return new MultiNode(nodeTypes.ToArray());
        }

        private Field[] GetPhoneticSequenceNodeFields(PhoneticSequence phonSeq)
        {
            var fields = new List<Field>();

            fields.Add(new Field(Format(PhoneticSequenceField.Phonemes), phonSeq.Phonemes.Format()));
            fields.Add(new Field(Format(PhoneticSequenceField.Syllables), phonSeq.Syllables.Format()));
            fields.Add(new Field(Format(PhoneticSequenceField.SyllableCount), phonSeq.Syllables.Length));

            return fields.ToArray();
        }


        public MultiNode Build(WordForm wordForm)
        {
            var nodeTypes = new List<NodeType>();

            nodeTypes.Add(new NodeType(
                label: LABEL_WORD_FORM,
                fields: GetWordFormNodeFields(wordForm)));

            nodeTypes.Add(new NodeType(label: GetLabel(wordForm.GlaffEntry.POS)));

            if (wordForm.IsLemma)
                nodeTypes.Add(new NodeType(label: LABEL_LEMMA));

            return new MultiNode(nodeTypes.ToArray());
        }

        private const string HISTORY = "history";
        private const string CHANGE_COUNT = "change_count";

        private Field[] GetWordFormNodeFields(WordForm wordForm)
        {
            var fields = new List<Field>();
            var entry = wordForm.GlaffEntry;

            fields.Add(new Field(Format(WordFormField.Rank), entry.Rank));
            fields.Add(new Field(Format(WordFormField.Graphemes), entry.GraphicalForm));

            fields.Add(new Field(Format(WordFormField.PronunciationRaw), entry.IpaPronunciations));
            fields.Add(new Field(Format(WordFormField.Pronunciation), wordForm.Pronunciation));
            fields.Add(new Field($"{Format(WordFormField.Pronunciation)}.{HISTORY}", wordForm.PronunciationHistory.Format()));
            fields.Add(new Field($"{Format(WordFormField.Pronunciation)}.{CHANGE_COUNT}", wordForm.PronunciationHistory.ChangeCount));

            if (wordForm.Phonemes != null)
                fields.Add(new Field(Format(WordFormField.Phonemes), wordForm.Phonemes.Format()));

            if (wordForm.Alignment != null)
                fields.Add(new Field(Format(WordFormField.Alignment), wordForm.Alignment));

            if (wordForm.Syllables != null)
            {
                fields.Add(new Field(Format(WordFormField.Syllables), wordForm.Syllables.Format()));
                fields.Add(new Field(Format(WordFormField.SyllableCount), wordForm.Syllables.Length));
            }

            //if (wordForm.Rhymes != null)
            //{
            //    fields.Add(new Field(Format(WordFormField.FinalRhyme), wordForm.Rhymes.FinalRhyme.Format()));
            //    for (int i = 0; i < wordForm.Rhymes.InnerRhymes.Length; i++)
            //        fields.Add(new Field($"{Format(WordFormField.InnerRhyme)}{i+1}", wordForm.Rhymes.InnerRhymes[i].Format()));
            //}

            fields.Add(new Field(Format(WordFormField.Lemma), entry.Lemma));

            if (entry.Number.HasValue)
                fields.Add(new Field(Format(WordFormField.Number), Format(entry.Number.Value)));

            if (entry.Gender.HasValue)
                fields.Add(new Field(Format(WordFormField.Gender), Format(entry.Gender.Value)));

            if (entry.Person.HasValue)
                fields.Add(new Field(Format(WordFormField.Person), Format(entry.Person.Value)));

            if (entry.Mood.HasValue)
                fields.Add(new Field(Format(WordFormField.Mood), Format(entry.Mood.Value)));

            foreach (var anomaly in wordForm.Anomalies)
                fields.Add(new Field($"{Format(WordFormField.Anomalies)}.{Format(anomaly.Type)}", true));

            return fields.ToArray();
        }

        public Relationship BuildLemmaRelation(long originId, long targetId)
        {
            return new Relationship(
                originId, targetId,
                Format(RelationshipType.Lemma));
        }

        private string Format(InnerRhymeField field)
        {
            switch (field)
            {
                case InnerRhymeField.SyllableNumber: return "syllable_nb";
                default:
                    throw new NotImplementedException($"Unsupported inner rhyme field [{field}].");
            }
        }

        private string Format(PhoneticRealizationField field)
        {
            switch (field)
            {
                case PhoneticRealizationField.IsStandard: return "is_std";
                default:
                    throw new NotImplementedException($"Unsupported phonetic realization field [{field}].");
            }
        }

        public string Format(PhoneticSequenceField field)
        {
            switch (field)
            {
                case PhoneticSequenceField.Phonemes: return "phonemes";
                case PhoneticSequenceField.Syllables: return "syllables";
                case PhoneticSequenceField.SyllableCount: return "syllable_count";
                default:
                    throw new NotImplementedException($"Unsupported phonetic sequence field [{field}].");
            }
        }

        public string Format(WordFormField field)
        {
            switch (field)
            {
                case WordFormField.Rank: return "glaff.rank";
                case WordFormField.Graphemes: return "graphemes";

                case WordFormField.PronunciationRaw: return "glaff.pronunciation_raw";
                case WordFormField.Pronunciation: return "glaff.pronunciation";
                case WordFormField.PronunciationDebug: return "glaff.pronunciation.debug";
                case WordFormField.Phonemes: return "phon.phonemes";
                case WordFormField.Syllables: return "phon.syllables";
                case WordFormField.SyllableCount: return "phon.syllable_count";
                case WordFormField.Alignment: return "phon.alignment";
                case WordFormField.FinalRhyme: return "rhym.final";
                case WordFormField.InnerRhyme: return "rhym.inner";

                case WordFormField.Lemma: return "gram.lemma";
                case WordFormField.POS: return "gram.pos";
                case WordFormField.Number: return "gram.number";
                case WordFormField.Gender: return "gram.gender";
                case WordFormField.Person: return "gram.person";
                case WordFormField.Mood: return "gram.mood";

                case WordFormField.Anomalies: return "anomaly";
                default:
                    throw new NotImplementedException($"Unsupported word form field [{field}].");
            }
        }

        private string Format(POS pos)
        {
            switch (pos)
            {
                case POS.Adjective: return "adj";
                case POS.Adverb: return "adv";
                case POS.Conjunction: return "conj";
                case POS.Determiner: return "det";
                case POS.Interjection: return "int";
                case POS.Noun: return "noun";
                case POS.Pronoun: return "pro";
                case POS.Preposition: return "prep";
                case POS.Verb: return "verb";
                default:
                    throw new NotImplementedException($"Unsupported POS [{pos}].");
            }
        }

        private string GetLabel(POS pos)
        {
            switch (pos)
            {
                case POS.Adjective: return "Adjective";
                case POS.Adverb: return "Adverb";
                case POS.Conjunction: return "Conjonction";
                case POS.Determiner: return "Determiner";
                case POS.Interjection: return "Interjection";
                case POS.Noun: return "Noun";
                case POS.Pronoun: return "Pronoun";
                case POS.Preposition: return "Preposition";
                case POS.Verb: return "Verb";
                default:
                    throw new NotImplementedException($"Unsupported POS [{pos}].");
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
                case Mood.Participle: return "part";
                case Mood.Indicative: return "ind";
                case Mood.Subjunctive: return "subj";
                case Mood.Conditional: return "cond";
                case Mood.Imperative: return "imp";
                default:
                    throw new NotImplementedException($"Unsupported mood [{mood}].");
            }
        }

        private string Format(AnomalyType type)
        {
            switch (type)
            {
                case AnomalyType.NoPhoneme: return "no_phonemes";
                case AnomalyType.Acronym: return "acronym";
                case AnomalyType.MultiplePronunciations: return "multiple_pronunciations";
                case AnomalyType.BadSyllabation: return "bad_syllabation";
                case AnomalyType.AlignmentFailed: return "alignment_failed";
                case AnomalyType.SyllabificationError: return "syllabification_error";
                default:
                    throw new NotImplementedException($"Unsupported anomaly type [{type}].");
            }
        }

        private string Format(RelationshipType type)
        {
            switch (type)
            {
                case RelationshipType.Lemma: return "LEMMA";
                case RelationshipType.PhoneticRealization: return "PHONREAL";
                case RelationshipType.Rhyme: return "RHYME";
                case RelationshipType.InnerRhyme: return "INNER_RHYME";
                default:
                    throw new NotImplementedException($"Unsupported relationship type [{type}].");
            }
        }

    }
}
