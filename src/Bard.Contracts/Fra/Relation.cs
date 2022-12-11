using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public interface IRelation { }

    public class Relation<T> where T : IRelation
    {
        public long OriginId { get; }
        public long TargetId { get; }
        public T Content { get; }

        public Relation(long originId, long targetId, T content)
        {
            OriginId = originId;
            TargetId = targetId;
            Content = content;
        }
    }
}
