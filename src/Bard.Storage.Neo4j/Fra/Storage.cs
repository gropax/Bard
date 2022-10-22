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

        public async Task<PhonGraphWordDto[]> SearchPhonGraphWords(string graphemes, int limit = 10)
        {
            return await Transaction(async t =>
            {
                var cursor = await t.RunAsync($@"
                    MATCH (w:WordForm)-[r:PHONREAL]->(p:PhonSeq)
                    WHERE
                        w.graphemes STARTS WITH $graphemes AND
                        r.is_std = true AND
                        EXISTS(p.syllables)
                    RETURN w.graphemes AS graphemes, p.syllables AS syllables, count(w) AS count
                    ORDER BY graphemes, count DESC
                    LIMIT $limit",
                    new { graphemes, limit });

                var records = await cursor.ToListAsync();
                return records.Select(r =>
                {
                    string graphemes = r["graphemes"].As<string>();
                    string syllables = r["syllables"].As<string>();

                    return new PhonGraphWordDto(graphemes, syllables);

                }).ToArray();
            });
        }

        public Task<object> GetFinalRhymingWords(string graphemes, string phonemes, string sortDir, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<WordFormDto[]> SearchWordForms(string graphemes, int limit = 10)
        {
            return await Transaction(async t =>
            {
                var cursor = await t.RunAsync($@"
                    MATCH (w:WordForm)
                    WHERE w.graphemes STARTS WITH $graphemes
                    RETURN w
                    ORDER BY w.graphemes
                    LIMIT $limit",
                    new { graphemes, limit });

                var records = await cursor.ToListAsync();
                return records.Select(r =>
                {
                    var node = r["w"].As<INode>();
                    var labels = node.Labels;

                    string graphemes = node["graphemes"].As<string>();

                    string syllables = null;
                    Number? number = null;
                    Gender? gender = null;
                    Person? person = null;
                    Mood? mood = null;
                    Tense? tense = null;
                    
                    if (node.Properties.TryGetValue("phon.syllables", out var prop))
                        syllables = prop.As<string>();

                    var pos = Enum.Parse<POS>(node["gram.pos"].As<string>());
                    
                    if (node.Properties.TryGetValue("gram.number", out var numberProp))
                        number = Enum.Parse<Number>(numberProp.As<string>());
                    
                    if (node.Properties.TryGetValue("gram.gender", out var genderProp))
                        gender = Enum.Parse<Gender>(genderProp.As<string>());
                    
                    if (node.Properties.TryGetValue("gram.person", out var personProp))
                        person = Enum.Parse<Person>(personProp.As<string>());
                    
                    if (node.Properties.TryGetValue("gram.mood", out var moodProp))
                        mood = Enum.Parse<Mood>(moodProp.As<string>());
                    
                    if (node.Properties.TryGetValue("gram.tense", out var tenseProp))
                        tense = Enum.Parse<Tense>(tenseProp.As<string>());

                    return new WordFormDto(node.Id, graphemes, syllables, pos, number, gender, person, mood, tense);

                }).ToArray();
            });
        }

        public async Task DeleteAll(int batchSize = 1000)
        {
            await Transaction(async t => await t.RunAsync($@"CALL apoc.periodic.iterate('MATCH (n) RETURN n', 'DETACH DELETE n', {{batchSize: {batchSize}}})"));
        }

        public async Task DeleteLabel(string label, int batchSize = 1000)
        {
            await Transaction(async t =>
                await t.RunAsync($@"CALL apoc.periodic.iterate('MATCH (n:{label}) RETURN n', 'DETACH DELETE n', {{batchSize: {batchSize}}})"));
        }

        public async Task DeleteGlaffContent(int batchSize = 1000)
        {
            var q = $@"CALL apoc.periodic.iterate('MATCH (g:{NodeLabel.LABEL_GLAFF_ENTRY}) OPTIONAL MATCH (g)-[:{RelationshipLabel.HAS}*]->(p:{NodeLabel.LABEL_PRONUNCIATION}) RETURN g, p', 'DETACH DELETE g, p', {{batchSize: {batchSize}}})";
            await Transaction(async t => await t.RunAsync(q));
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
