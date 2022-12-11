using Bard.Contracts.Fra;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j
{
    public interface IRelationSerializer<T> where T : IRelation
    {
        Relationship Serialize(Relation<T> item);
        Relation<T> Deserialize(IRelationship node);
    }
}
