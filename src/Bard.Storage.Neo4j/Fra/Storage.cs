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

        public async Task CreateAsync(IEnumerable<MultiNode> multiNodes)
        {
            foreach (var grp in multiNodes.GroupBy(m => m.Labels))
            {
                await Transaction(async t =>
                {
                    var cursor = await t.RunAsync($@"
                        UNWIND $props AS map
                        CREATE (n{grp.Key})
                        SET n = map",
                        new { props = grp.Select(m => GetProperties(m)).ToArray() });
                });
            }
        }

        private Dictionary<string, object> GetProperties(MultiNode multiNode)
        {
            var props = new Dictionary<string, object>();

            foreach (var nodeType in multiNode.Types)
                foreach (var field in nodeType.Fields)
                    props[field.Name] = field.Value;

            return props;
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
