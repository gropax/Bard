using Bard.Contracts.Fra;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public class RhymeRelationSerializer : SerializerBase, IRelationSerializer<RhymeRelation>
    {
        public Relationship Serialize(Relation<RhymeRelation> item)
        {
            var fields = new List<Field>();

            return new Relationship(
                item.OriginId,
                item.TargetId,
                RelationshipLabel.RHYME,
                fields.ToArray());
        }

        public Relation<RhymeRelation> Deserialize(IRelationship rel)
        {
            EnsureHasType(rel, RelationshipLabel.RHYME);

            return new Relation<RhymeRelation>(
                rel.StartNodeId,
                rel.EndNodeId,
                new RhymeRelation());
        }

    }
}
