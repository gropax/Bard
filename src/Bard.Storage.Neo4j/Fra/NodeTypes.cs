using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public enum WordFormFields
    {
        Graphemes,
    }

    public class WordFormNodeType : INodeType
    {
        public string Graphemes { get; }

        public WordFormNodeType(string graphemes)
        {
            Graphemes = graphemes;
        }

        public IField[] Fields => new[]
        {
            new Field<WordFormFields>(WordFormFields.Graphemes, Graphemes),
        };
    }
}
