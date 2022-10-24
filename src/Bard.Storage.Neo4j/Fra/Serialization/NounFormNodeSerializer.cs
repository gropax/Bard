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
    public class NounFormNodeSerializer : NodeSerializerBase, INodeSerializer<NounForm>
    {
        public const string GRAPHICAL_FORM = "word_form.graphical_form";
        public const string LEMMA = "word_form.lemma";
        public const string POS = "word_form.pos";
        public const string GENDER = "word_form.gender";
        public const string NUMBER = "word_form.number";

        public MultiNode Serialize(NounForm item)
        {
            var nodeTypes = new List<NodeType>();
            var fields = new List<Field>();

            fields.Add(new Field(GRAPHICAL_FORM, item.GraphicalForm));
            fields.Add(new Field(LEMMA, item.Lemma));
            fields.Add(GetField(POS, item.POS));

            if (item.Gender.HasValue)
                fields.Add(GetField(GENDER, item.Gender.Value));

            if (item.Number.HasValue)
                fields.Add(GetField(NUMBER, item.Number.Value));


            nodeTypes.Add(new NodeType(
                label: NodeLabel.WORD_FORM,
                fields: fields.ToArray()));

            nodeTypes.Add(new NodeType(label: NodeLabel.NOUN)); // @fixme

            return new MultiNode(nodeTypes.ToArray());
        }

        public NounForm Deserialize(INode node)
        {
            EnsureHasLabels(node, NodeLabel.WORD_FORM);
            EnsureHasProperty(node, POS, Contracts.Fra.POS.Noun.ToString());

            var graphicalForm = node[GRAPHICAL_FORM].As<string>();
            var lemma = node[LEMMA].As<string>();

            Gender? gender = null;
            if (node.Properties.TryGetValue(GENDER, out var genderObj))
                gender = Enum.Parse<Gender>(genderObj.As<string>());

            Number? number = null;
            if (node.Properties.TryGetValue(NUMBER, out var numberObj))
                number = Enum.Parse<Number>(numberObj.As<string>());

            return new NounForm()
            {
                Id = node.Id,
                GraphicalForm = graphicalForm,
                Lemma = lemma,
                Gender = gender,
                Number = number,
            };
        }
    }
}
