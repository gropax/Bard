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
        Phonemes,
        Lemma,
        POS,
        Number,
        Gender,
        Person,
        Mood,
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
