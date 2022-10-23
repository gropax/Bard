using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j
{
    public abstract class NodeSerializerBase
    {
        protected Field GetField<T>(string fieldName, T value) where T : Enum
        {
            return new Field(fieldName, value.ToString());
        }

        protected void EnsureHasLabel(INode node, params string[] labels)
        {
            foreach (var label in labels)
            {
                if (!node.Labels.Contains(label))
                {
                    string nodeLabels = string.Join(", ", node.Labels);
                    throw new Exception($"Node should have label [{label}] but has [{nodeLabels}].");
                }
            }
        }
    }
}
