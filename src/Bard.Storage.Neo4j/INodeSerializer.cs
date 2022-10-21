using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j
{
    public interface INodeSerializer<T>
    {
        MultiNode Serialize(T item);
        T Deserialize(MultiNode node);
    }
}
