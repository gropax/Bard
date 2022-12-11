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
    public class PronunciationNodeSerializer : NodeSerializerBase, INodeSerializer<Pronunciation>
    {
        public MultiNode Serialize(Pronunciation item)
        {
            var nodeTypes = new List<NodeType>();
            var fields = new List<Field>();

            fields.Add(new Field(PropLabel.PRONUN_GRAPHICAL_FORM, item.Graphemes));
            fields.Add(new Field(PropLabel.PRONUN_ORIGINAL_VALUE, item.History.InitialValue));
            fields.Add(new Field(PropLabel.PRONUN_FINAL_VALUE, item.Value));
            fields.Add(new Field(PropLabel.PRONUN_CHANGE_HISTORY, item.History.Format()));
            fields.Add(new Field(PropLabel.PRONUN_PHONEMES, string.Join('.', item.Phonemes)));
            fields.Add(new Field(PropLabel.PRONUN_ALIGNMENT, item.Alignment));
            fields.Add(new Field(PropLabel.PRONUN_ANOMALY_COUNT, item.Anomalies.Count));
            fields.Add(new Field(PropLabel.PRONUN_ANOMALIES, string.Join(',', item.Anomalies.Select(a => a.Type.ToString()))));
            fields.Add(new Field(PropLabel.PRONUN_IS_VALID, item.IsValid));


            nodeTypes.Add(new NodeType(
                label: NodeLabel.PRONUNCIATION,
                fields: fields.ToArray()));


            return new MultiNode(nodeTypes.ToArray());
        }

        public Pronunciation Deserialize(INode node)
        {
            EnsureHasLabels(node, NodeLabel.PRONUNCIATION);

            var graphicalForm = node[PropLabel.PRONUN_GRAPHICAL_FORM].As<string>();
            var finalValue = node[PropLabel.PRONUN_FINAL_VALUE].As<string>();

            string phonemes = null;
            if (node.Properties.TryGetValue(PropLabel.PRONUN_PHONEMES, out var phonemesObj))
                phonemes = phonemesObj.As<string>();

            string alignment = null;
            if (node.Properties.TryGetValue(PropLabel.PRONUN_ALIGNMENT, out var alignmentObj))
                alignment = alignmentObj.As<string>();

            bool isValid = node[PropLabel.PRONUN_IS_VALID].As<bool>();

            //var originalValue = node[PropLabel.ORIGINAL_VALUE].As<string>();
            //var changeHistory = node[PropLabel.CHANGE_HISTORY].As<string>();
            //var anomalyCount = node[PropLabel.ANOMALY_COUNT].As<int>();
            //var anomalies = node[PropLabel.ANOMALIES].As<string>();

            return new Pronunciation()
            {
                Id = node.Id,
                Graphemes = graphicalForm,
                Value = finalValue,
                Phonemes = phonemes.Split('.'),
                Alignment = alignment,
                IsValid = isValid,
            };
        }
    }
}
