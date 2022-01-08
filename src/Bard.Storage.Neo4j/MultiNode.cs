using System;
using System.Linq;

namespace Bard.Storage.Neo4j
{
    public interface IMultiNode
    {
        public INodeType[] Types { get; }
        public int TypeHash { get; }
    }

    public interface INodeType
    {
        public IField[] Fields { get; }
    }

    public interface IField
    {
        public object Value { get; }
    }

    public class MultiNode : IMultiNode
    {
        public INodeType[] Types { get; }

        public int TypeHash => Types.Sum(t => t.GetType().GetHashCode());

        public MultiNode(params INodeType[] types)
        {
            Types = types;
        }
    }

    //public abstract class NodeType<T> : INodeType where T : Enum
    //{
    //    IField[] INodeType.Fields => Fields;
    //    public abstract Field<T>[] Fields { get; }
    //}

    public class Field<T> : IField where T : Enum
    {
        public T Key { get; }
        public object Value { get; }

        public Field(T key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
