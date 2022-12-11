using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j
{
    public abstract class SerializerBase
    {
        protected Field GetField<T>(string fieldName, T value) where T : Enum
        {
            return new Field(fieldName, value.ToString());
        }

        protected void EnsureHasLabels(INode node, params string[] labels)
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

        protected void EnsureHasProperty(INode node, string prop, object expectedValue)
        {
            if (!node.Properties.TryGetValue(prop, out var value))
                throw new Exception($"Node should have property [{prop}].");

            if (value != expectedValue)
                throw new Exception($"Node property [{prop}] should be [{expectedValue}] but was [{value}].");
        }

        protected void EnsureHasType(IRelationship rel, string type)
        {
            if (rel.Type != type)
                throw new Exception($"Relationship should have type [{type}] but has [{rel.Type}].");
        }

    }
}
