using Bard.Contracts.Fra;
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

        public async Task<WordForm[]> SearchWordForms(string graphemes, int limit = 10)
        {
            return await Transaction(async t =>
            {
                var cursor = await t.RunAsync($@"
                    MATCH (w:WordForm)
                    WHERE w.graphemes STARTS WITH $graphemes
                    RETURN w
                    LIMIT $limit",
                    new { graphemes, limit });

                var records = await cursor.ToListAsync();
                return records.Select(r =>
                {
                    var node = r["w"].As<INode>();
                    string graphemes = node["graphemes"].As<string>();
                    string syllables = null;

                    if (node.Properties.TryGetValue("phon.syllables", out var prop))
                        syllables = prop.As<string>();

                    return new WordForm(graphemes, syllables);
                }).ToArray();
            });
        }

        public async Task DeleteAll(int batchSize = 1000)
        {
            await Transaction(async t => await t.RunAsync($@"CALL apoc.periodic.iterate('MATCH (n) RETURN n', 'DETACH DELETE n', {{batchSize: {batchSize}}})"));
        }

        public async Task<long[]> CreateAsync(IEnumerable<Relationship> relationships)
        {
            var ids = new List<long>();

            foreach (var grp in relationships.GroupBy(r => r.Label))
            {
                await Transaction(async t =>
                {
                    var props = grp.Select(m => GetProperties(m)).ToArray();
                    var cursor = await t.RunAsync($@"
                        UNWIND $props AS map
                        MATCH (o) WHERE id(o) = map._originId
                        MATCH (t) WHERE id(t) = map._targetId
                        MERGE (o)-[r:{grp.Key}]->(t)
                        WITH r, apoc.map.removeKeys(map, ['_originId', '_targetId']) AS fields
                        SET r = fields
                        RETURN r",
                        new { props = props });

                    var records = await cursor.ToListAsync();
                    ids.AddRange(records.Select(r => r["r"].As<IRelationship>().Id));
                });
            }

            return ids.ToArray();
        }

        public async Task<INode[]> CreateAsync(IEnumerable<MultiNode> multiNodes)
        {
            var ids = new List<INode>();

            foreach (var grp in multiNodes.GroupBy(m => m.Labels))
            {
                await Transaction(async t =>
                {
                    var props = grp.Select(m => GetProperties(m)).ToArray();
                    var cursor = await t.RunAsync($@"
                        UNWIND $props AS map
                        CREATE (n{grp.Key})
                        SET n = map
                        RETURN n",
                        new { props });

                    var records = await cursor.ToListAsync();
                    ids.AddRange(records.Select(r => r["n"].As<INode>()));
                });
            }

            return ids.ToArray();
        }

        private Dictionary<string, object> GetProperties(Relationship r)
        {
            var props = new Dictionary<string, object>();

            props["_originId"] = r.OriginId;
            props["_targetId"] = r.TargetId;

            foreach (var field in r.Fields)
                props[field.Name] = field.Value;

            return props;
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
