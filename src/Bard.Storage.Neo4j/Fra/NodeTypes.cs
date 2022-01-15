using Bard.Contracts.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public enum PhoneticSequenceField
    {
        Phonemes,
        Syllables,
        SyllableCount,
    }

    public enum WordFormField
    {
        Rank,
        Graphemes,

        PronunciationRaw,
        Pronunciation,
        PronunciationDebug,
        Phonemes,
        Alignment,
        Syllables,
        SyllableCount,
        FinalRhyme,
        InnerRhyme,

        Lemma,
        POS,
        Number,
        Gender,
        Person,
        Mood,

        Anomalies,
    }

    public enum PhoneticRealizationField
    {
        IsStandard,
    }

    public enum RelationshipType
    {
        Lemma,
        PhoneticRealization,
    }

    public class NodeType
    {
        public string Label { get; }
        public Field[] Fields { get; }

        public NodeType(string label, params Field[] fields)
        {
            Label = label;
            Fields = fields;
        }
    }
}
