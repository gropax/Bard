using Bard.Contracts.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public enum WordFormField
    {
        Graphemes,

        PronunciationRaw,
        Pronunciation,
        PronunciationDebug,
        Phonemes,
        Alignment,
        Syllables,
        SyllableCount,

        Lemma,
        POS,
        Number,
        Gender,
        Person,
        Mood,

        Anomalies,
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
