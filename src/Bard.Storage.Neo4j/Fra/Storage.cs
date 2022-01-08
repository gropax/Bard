using Bard.Storage.Fra;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public class GraphStorage : IStorage
    {
        private IDriver _driver;

        public GraphStorage(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public async Task CreateAsync(IEnumerable<IMultiNode> multiNodes)
        {
            foreach (var grp in multiNodes.GroupBy(m => m.TypeHash))
            {
                var labels = string.Join("", grp.First().Types
                    .Select(t => $":{GetNodeLabel(t)}")
                    .OrderBy(l => l));

                await Transaction(async t =>
                {
                    var cursor = await t.RunAsync($@"
                        UNWIND $props AS map
                        CREATE (n{labels})
                        SET n = map",
                        new { props = grp.Select(m => GetProperties(m)).ToArray() });
                });
            }
        }

        private Dictionary<Type, string> _nodeTypes = new Dictionary<Type, string>()
        {
            { typeof(WordFormNodeType), "WordForm" },
        };
        private string GetNodeLabel(INodeType nodeType)
        {
            if (_nodeTypes.TryGetValue(nodeType.GetType(), out string label))
                return label;
            else
                throw new NotImplementedException($"Unsupported node type [{nodeType.GetType()}].");
        }

        private Dictionary<string, object> GetProperties(IMultiNode multiNode)
        {
            var props = new Dictionary<string, object>();

            foreach (var nodeType in multiNode.Types)
                AddProperties(props, nodeType);

            return props;
        }

        private void AddProperties(Dictionary<string, object> props, INodeType nodeType)
        {
            foreach (var field in nodeType.Fields)
            {
                var fieldName = GetFieldName(field);
                props[fieldName] = field.Value;
            }
        }

        private Dictionary<WordFormFields, string> _wordFormFieldNames = new Dictionary<WordFormFields, string>()
        {
            { WordFormFields.Graphemes, "graphemes" },
        };
        private string GetFieldName(IField field)
        {
            switch (field)
            {
                case Field<WordFormFields> f: return GetFieldName(_wordFormFieldNames, f.Key);
                default:
                    throw new NotImplementedException($"Unsupported field type [{field.GetType()}].");
            }
        }

        private string GetFieldName<T>(Dictionary<T, string> nameDict, T key)
        {
            if (nameDict.TryGetValue(key, out var name))
                return name;
            else
                throw new NotImplementedException($"Unsupported field [{key}].");
        }


        private async Task Transaction(Func<IAsyncTransaction, Task> func)
        {
            var session = _driver.AsyncSession();
            var transaction = await session.BeginTransactionAsync();
            try
            {
                await func(transaction);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<T> Transaction<T>(Func<IAsyncTransaction, Task<T>> func)
        {
            var session = _driver.AsyncSession();
            var transaction = await session.BeginTransactionAsync();
            try
            {
                var result = await func(transaction);
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

    }
}
