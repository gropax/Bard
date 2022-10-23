using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public static class RelationshipLabel
    {
        public const string HAS = "HAS";
    }

    public static class NodeLabel
    {
        public const string GLAFF_ENTRY = "GlaffEntry";
        public const string PRONUNCIATION = "Pronun";

        public const string LEMMA = "Lemma";
        public const string WORD_FORM = "WordForm";

        public const string NOUN = "Noun";

        public const string PHONETIC_SEQUENCE = "PhonSeq";
    }

}
