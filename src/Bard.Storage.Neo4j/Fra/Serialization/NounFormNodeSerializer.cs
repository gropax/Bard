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
    public class WordFormNodeSerializer : NodeSerializerBase, INodeSerializer<WordForm2>
    {
        public MultiNode Serialize(WordForm2 item)
        {
            var nodeTypes = new List<NodeType>();
            var fields = new List<Field>();

            fields.Add(new Field(PropLabel.WORD_FORM_GRAPHICAL_FORM, item.GraphicalForm));
            fields.Add(new Field(PropLabel.WORD_FORM_LEMMA, item.Lemma));
            fields.Add(GetField(PropLabel.WORD_FORM_POS, item.POS));

            nodeTypes.Add(new NodeType(
                label: NodeLabel.WORD_FORM,
                fields: fields.ToArray()));

            nodeTypes.Add(new NodeType(label: NodeLabel.NOUN)); // @fixme

            return new MultiNode(nodeTypes.ToArray());
        }

        public WordForm2 Deserialize(INode node)
        {
            EnsureHasLabels(node, NodeLabel.WORD_FORM);

            var graphicalForm = node[PropLabel.WORD_FORM_GRAPHICAL_FORM].As<string>();
            var lemma = node[PropLabel.WORD_FORM_LEMMA].As<string>();

            return new WordForm2()
            {
                Id = node.Id,
                GraphicalForm = graphicalForm,
                Lemma = lemma,
            };
        }
    }


    public class NounFormNodeSerializer : NodeSerializerBase, INodeSerializer<NounForm>
    {
        public MultiNode Serialize(NounForm item)
        {
            var nodeTypes = new List<NodeType>();
            var fields = new List<Field>();

            fields.Add(new Field(PropLabel.WORD_FORM_GRAPHICAL_FORM, item.GraphicalForm));
            fields.Add(new Field(PropLabel.WORD_FORM_LEMMA, item.Lemma));
            fields.Add(GetField(PropLabel.WORD_FORM_POS, item.POS));

            if (item.Gender.HasValue)
                fields.Add(GetField(PropLabel.WORD_FORM_GENDER, item.Gender.Value));

            if (item.Number.HasValue)
                fields.Add(GetField(PropLabel.WORD_FORM_NUMBER, item.Number.Value));


            nodeTypes.Add(new NodeType(
                label: NodeLabel.WORD_FORM,
                fields: fields.ToArray()));

            nodeTypes.Add(new NodeType(label: NodeLabel.NOUN)); // @fixme

            return new MultiNode(nodeTypes.ToArray());
        }

        public NounForm Deserialize(INode node)
        {
            EnsureHasLabels(node, NodeLabel.WORD_FORM);
            EnsureHasProperty(node, PropLabel.WORD_FORM_POS, POS.Noun.ToString());

            var graphicalForm = node[PropLabel.WORD_FORM_GRAPHICAL_FORM].As<string>();
            var lemma = node[PropLabel.WORD_FORM_LEMMA].As<string>();

            Gender? gender = null;
            if (node.Properties.TryGetValue(PropLabel.WORD_FORM_GENDER, out var genderObj))
                gender = Enum.Parse<Gender>(genderObj.As<string>());

            Number? number = null;
            if (node.Properties.TryGetValue(PropLabel.WORD_FORM_NUMBER, out var numberObj))
                number = Enum.Parse<Number>(numberObj.As<string>());

            return new NounForm()
            {
                Id = node.Id,
                POS = POS.Noun,
                GraphicalForm = graphicalForm,
                Lemma = lemma,
                Gender = gender,
                Number = number,
            };
        }
    }
}
