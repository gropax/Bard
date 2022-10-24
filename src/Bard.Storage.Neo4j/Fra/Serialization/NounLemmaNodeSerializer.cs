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
    public class NounLemmaNodeSerializer : NodeSerializerBase, INodeSerializer<NounLemma>
    {
        public const string GRAPHICAL_FORM = "lemma.graphical_form";
        public const string POS = "lemma.pos";
        public const string GENDER = "lemma.gender";

        public MultiNode Serialize(NounLemma item)
        {
            var nodeTypes = new List<NodeType>();
            var fields = new List<Field>();

            fields.Add(new Field(GRAPHICAL_FORM, item.GraphicalForm));
            fields.Add(GetField(POS, item.POS));

            if (item.Gender.HasValue)
                fields.Add(GetField(GENDER, item.Gender.Value));


            nodeTypes.Add(new NodeType(
                label: NodeLabel.LEMMA,
                fields: fields.ToArray()));

            nodeTypes.Add(new NodeType(label: NodeLabel.NOUN)); // @fixme

            return new MultiNode(nodeTypes.ToArray());
        }

        public NounLemma Deserialize(INode node)
        {
            EnsureHasLabels(node, NodeLabel.LEMMA);
            EnsureHasProperty(node, POS, Contracts.Fra.POS.Noun.ToString());

            var graphicalForm = node[GRAPHICAL_FORM].As<string>();

            Gender? gender = null;
            if (node.Properties.TryGetValue(GENDER, out var genderObj))
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
