using Bard.Contracts.Fra;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using Bard.Utils;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public class PhoneticSequenceNodeSerializer : SerializerBase, INodeSerializer<PhoneticSequence>
    {
        public MultiNode Serialize(PhoneticSequence item)
        {
            var nodeTypes = new List<NodeType>();
            var fields = new List<Field>();

            fields.Add(new Field(PropLabel.PHONETIC_SEQUENCE_IPA, item.IpaRepresentation));
            fields.Add(new Field(PropLabel.PHONETIC_SEQUENCE_PHONEMES, string.Join('.', item.Phonemes.Select(p => p.Symbol))));
            fields.Add(new Field(PropLabel.PHONETIC_SEQUENCE_SYLLABLES, string.Join('.', item.Syllables.Select(s => s.Format()))));

            nodeTypes.Add(new NodeType(
                label: NodeLabel.PHONETIC_SEQUENCE,
                fields: fields.ToArray()));

            return new MultiNode(nodeTypes.ToArray());
        }

        public PhoneticSequence Deserialize(INode node)
        {
            EnsureHasLabels(node, NodeLabel.PHONETIC_SEQUENCE);

            var ipaRepr = node[PropLabel.PHONETIC_SEQUENCE_IPA].As<string>();

            var phonemesStr = node[PropLabel.PHONETIC_SEQUENCE_PHONEMES].As<string>();
            var phonemes = phonemesStr.Split('.').Select(s => Phonemes.BySymbol(s)).ToArray();

            var syllablesStr = node[PropLabel.PHONETIC_SEQUENCE_SYLLABLES].As<string>();
            //var syllables = syllablesStr.Split('.')
            //    .Select(s =>
            //        new Syllable()
            //    IpaHelpers.ParseSymbols(s).Select(s => Phonemes.BySymbol(s)).ToArray();

            return new PhoneticSequence()
            {
                Id = node.Id,
                IpaRepresentation = ipaRepr,
                Phonemes = phonemes,
                Syllables = null,
            };
        }
    }
}
