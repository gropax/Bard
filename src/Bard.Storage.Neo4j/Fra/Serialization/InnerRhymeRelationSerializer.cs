using Bard.Contracts.Fra;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public class InnerRhymeRelationSerializer : SerializerBase, IRelationSerializer<InnerRhymeRelation>
    {
        public Relationship Serialize(Relation<InnerRhymeRelation> item)
        {
            var fields = new List<Field>();

            return new Relationship(
                item.OriginId,
                item.TargetId,
                RelationshipLabel.INNER_RHYME,
                fields.ToArray());
        }

        public Relation<InnerRhymeRelation> Deserialize(IRelationship rel)
        {
            EnsureHasType(rel, RelationshipLabel.INNER_RHYME);

            return new Relation<InnerRhymeRelation>(
                rel.StartNodeId,
                rel.EndNodeId,
                new InnerRhymeRelation());
        }

    }
}
