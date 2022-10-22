using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Glaff
{
    public class PronunciationNodeSerializer : INodeSerializer<Pronunciation>
    {
        public const string GRAPHICAL_FORM = "pronun.raw.graphical_form";
        public const string ORIGINAL_VALUE = "pronun.raw.original_value";
        public const string FINAL_VALUE = "pronun.calc.final_value";
        public const string CHANGE_HISTORY = "pronun.calc.change_history";
        public const string PHONEMES = "pronun.calc.phonemes";
        public const string ALIGNMENT = "pronun.calc.alignment";
        public const string ANOMALY_COUNT = "pronun.calc.anomaly_count";
        public const string ANOMALIES = "pronun.calc.anomalies";
        public const string IS_VALID = "pronun.calc.is_valid";

        public MultiNode Serialize(Pronunciation item)
        {
            var nodeTypes = new List<NodeType>();
            var fields = new List<Field>();

            fields.Add(new Field(GRAPHICAL_FORM, item.Graphemes));
            fields.Add(new Field(ORIGINAL_VALUE, item.History.InitialValue));
            fields.Add(new Field(FINAL_VALUE, item.Value));
            fields.Add(new Field(CHANGE_HISTORY, item.History.Format()));
            fields.Add(new Field(PHONEMES, string.Join('.', item.Phonemes)));
            fields.Add(new Field(ALIGNMENT, item.Alignment));
            fields.Add(new Field(ANOMALY_COUNT, item.Anomalies.Count));
            fields.Add(new Field(ANOMALIES, string.Join(',', item.Anomalies.Select(a => a.Type.ToString()))));
            fields.Add(new Field(IS_VALID, item.IsValid));


            nodeTypes.Add(new NodeType(
                label: NodeLabel.LABEL_PRONUNCIATION,
                fields: fields.ToArray()));


            return new MultiNode(nodeTypes.ToArray());
        }

        public Pronunciation Deserialize(MultiNode node)
        {
            throw new NotImplementedException();
        }

        protected Field GetField<T>(string fieldName, T value) where T : Enum
        {
            return new Field(fieldName, value.ToString());
        }
    }
}
