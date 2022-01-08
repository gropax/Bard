using Bard.Storage.Neo4j.Fra;
using System;
using System.Linq;

namespace Bard.Storage.Neo4j
{
    public class MultiNode
    {
        public NodeType[] Types { get; }
        public string Labels => string.Join(string.Empty, Types.Select(t => $":{t.Label}"));

        public MultiNode(params NodeType[] types)
        {
            Types = types;
        }
    }

    public class Field
    {
        public string Name { get; }
        public object Value { get; }

        public Field(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
