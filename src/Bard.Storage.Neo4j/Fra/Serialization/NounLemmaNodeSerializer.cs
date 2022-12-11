using Bard.Contracts.Fra;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public class NounLemmaNodeSerializer : SerializerBase, INodeSerializer<NounLemma>
    {
        public MultiNode Serialize(NounLemma item)
        {
            var nodeTypes = new List<NodeType>();
            var fields = new List<Field>();

            fields.Add(new Field(PropLabel.LEMMA_GRAPHICAL_FORM, item.GraphicalForm));
            fields.Add(GetField(PropLabel.LEMMA_POS, item.POS));

            if (item.Gender.HasValue)
                fields.Add(GetField(PropLabel.LEMMA_GENDER, item.Gender.Value));


            nodeTypes.Add(new NodeType(
                label: NodeLabel.LEMMA,
                fields: fields.ToArray()));

            nodeTypes.Add(new NodeType(label: NodeLabel.NOUN)); // @fixme

            return new MultiNode(nodeTypes.ToArray());
        }

        public NounLemma Deserialize(INode node)
        {
            EnsureHasLabels(node, NodeLabel.LEMMA);
            EnsureHasProperty(node, PropLabel.LEMMA_POS, POS.Noun.ToString());

            var graphicalForm = node[PropLabel.LEMMA_GRAPHICAL_FORM].As<string>();

            Gender? gender = null;
            if (node.Properties.TryGetValue(PropLabel.LEMMA_GENDER, out var genderObj))
                gender = Enum.Parse<Gender>(genderObj.As<string>());

            return new NounLemma()
            {
                Id = node.Id,
                GraphicalForm = graphicalForm,
                Gender = gender,
            };
        }
    }
}
