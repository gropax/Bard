using Bard.Contracts.Fra;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public class PhoneticRealizationRelationSerializer : SerializerBase, IRelationSerializer<PhoneticRealizationRelation>
    {
        public Relationship Serialize(Relation<PhoneticRealizationRelation> item)
        {
            var fields = new List<Field>();

            return new Relationship(
                item.OriginId,
                item.TargetId,
                RelationshipLabel.PHONETIC_REALIZATION,
                fields.ToArray());
        }

        public Relation<PhoneticRealizationRelation> Deserialize(IRelationship rel)
        {
            EnsureHasType(rel, RelationshipLabel.PHONETIC_REALIZATION);

            return new Relation<PhoneticRealizationRelation>(
                rel.StartNodeId,
                rel.EndNodeId,
                new PhoneticRealizationRelation());
        }

    }
}
