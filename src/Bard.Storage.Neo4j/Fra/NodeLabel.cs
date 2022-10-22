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
        public const string LABEL_WORD_FORM = "WordForm";
        public const string LABEL_PHON_SEQ = "PhonSeq";
        public const string LABEL_LEMMA = "Lemma";
        public const string LABEL_GLAFF_ENTRY = "GlaffEntry";
        public const string LABEL_PRONUNCIATION = "Pronun";
    }

}
