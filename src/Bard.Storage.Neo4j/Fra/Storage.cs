using Bard.Contracts.Fra;
using Bard.Contracts.Fra.Analysis.Phonology;
using Bard.Contracts.Fra.Analysis.Words;
using Bard.Storage.Fra;
using Bard.Utils;
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

        private GlaffEntryNodeSerializer _glaffEntryNodeSerializer = new GlaffEntryNodeSerializer();
        private PronunciationNodeSerializer _pronunciationNodeSerializer = new PronunciationNodeSerializer();
        private NounLemmaNodeSerializer _nounLemmaNodeSerializer = new NounLemmaNodeSerializer();
        private WordFormNodeSerializer _wordFormNodeSerializer = new WordFormNodeSerializer();
        private NounFormNodeSerializer _nounFormNodeSerializer = new NounFormNodeSerializer();
        private PhoneticSequenceNodeSerializer _phoneticSequenceNodeSerializer = new PhoneticSequenceNodeSerializer();
        private PhoneticRealizationRelationSerializer _phoneticRealizationRelationSerializer = new PhoneticRealizationRelationSerializer();
        private RhymeRelationSerializer _rhymeRelationSerializer = new RhymeRelationSerializer();
        private InnerRhymeRelationSerializer _innerRhymeRelationSerializer = new InnerRhymeRelationSerializer();

        /// <summary>
        /// Return all nominal word forms grouped by their lemma.
        /// </summary>
        public async IAsyncEnumerable<LemmaData<NounLemma, NounForm>> GetNounLemmaData()
        {
            var session = _driver.AsyncSession();
            try
            {
                var cursor = await session.RunAsync($@"
                    MATCH (g:{NodeLabel.GLAFF_ENTRY})
                    WHERE g.`{PropLabel.GLAFF_POS}` = '{POS.Noun}'
                    OPTIONAL MATCH (g)-[:{RelationshipLabel.HAS}]->(p:{NodeLabel.PRONUNCIATION})
                    WITH g, collect(p) AS px
                    WITH
                        g.`{PropLabel.GLAFF_LEMMA}` AS lemma,
                        g.`{PropLabel.GLAFF_GENDER}` AS gender,
                        g.`{PropLabel.GLAFF_NUMBER}` AS number, 
                        collect({{ entry: g, pronunciations: px }}) AS entries
                    RETURN lemma, gender, collect(entries) AS entries");

                while (await cursor.FetchAsync())
                {
                    var r = cursor.Current;
                    var entries = r["entries"].As<List<object>>();

                    var wordForms = new List<WordFormData<NounForm>>();
                    foreach (var wfDataObj in entries)
                    {
                        var glaffEntries = new List<GlaffEntry>();

                        var wordFormData = wfDataObj.As<List<object>>();
                        foreach (var entryData in wordFormData)
                        {
                            var dict = entryData.As<Dictionary<string, object>>();
                            var entry = dict["entry"].As<INode>();
                            var pronunciations = dict["pronunciations"].As<List<INode>>();

                            var glaffEntry = _glaffEntryNodeSerializer.Deserialize(entry);
                            glaffEntry.Pronunciations = pronunciations
                                .Select(p => _pronunciationNodeSerializer.Deserialize(p))
                                .ToArray();

                            glaffEntries.Add(glaffEntry);
                        }

                        wordForms.Add(new WordFormData<NounForm>(glaffEntries.ToArray()));
                    }

                    yield return new LemmaData<NounLemma, NounForm>(wordForms.ToArray());
                }
            }
            finally
            {
                await session.CloseAsync();
            }
        }


        public async Task CreateNounsAsync(
            IEnumerable<LemmaData<NounLemma, NounForm>> nouns)
        {
            foreach (var batch in nouns.Batch(1000))
            {
                var lemmaMultiNodes = batch
                    .Select(e => _nounLemmaNodeSerializer.Serialize(e.Lemma));

                var lemmaNodes = await CreateAsync(lemmaMultiNodes);
                var lemma2Id = Enumerable.Range(0, batch.Length)
                    .ToDictionary(i => batch[i].Lemma, i => lemmaNodes[i].Id);

                var formBatch = batch
                    .SelectMany(e => e.WordForms)
                    .Select(f => f.WordForm)
                    .ToArray();

                var formMultiNodes = formBatch.Select(p => _nounFormNodeSerializer.Serialize(p));

                var formNodes = await CreateAsync(formMultiNodes);
                var form2Id = Enumerable.Range(0, formBatch.Length)
                    .ToDictionary(i => formBatch[i], i => formNodes[i].Id);

                var lemmaRels =
                    from lemmaData in batch
                    from wordForm in lemmaData.WordForms
                    select new Relationship(
                        originId: form2Id[wordForm.WordForm],
                        targetId: lemma2Id[lemmaData.Lemma],
                        label: RelationshipLabel.LEMMA);

                await CreateAsync(lemmaRels);

                var pronunciationRels =
                    from lemmaData in batch
                    from wordForm in lemmaData.WordForms
                    from pronunciation in wordForm.Pronunciations
                    select new Relationship(
                        originId: form2Id[wordForm.WordForm],
                        targetId: pronunciation.Id.Value,
                        label: RelationshipLabel.HAS);

                await CreateAsync(pronunciationRels);

                var glaffEntryRels =
                    from lemmaData in batch
                    from wordForm in lemmaData.WordForms
                    from glaffEntry in wordForm.GlaffEntries
                    select new Relationship(
                        originId: form2Id[wordForm.WordForm],
                        targetId: glaffEntry.Id.Value,
                        label: RelationshipLabel.SOURCE);

                await CreateAsync(glaffEntryRels);
            }
        }

        public async Task CreateGlaffEntriesAsync(
            IEnumerable<GlaffEntry> entries)
        {
            foreach (var entryBatch in entries.Batch(1000))
            {
                var entryMultinodes = entryBatch
                    .Select(e => _glaffEntryNodeSerializer.Serialize(e));

                var entryNodes = await CreateAsync(entryMultinodes);
                var entry2id = Enumerable.Range(0, entryBatch.Length)
                    .ToDictionary(i => entryBatch[i], i => entryNodes[i].Id);

                var pronunBatch = entryBatch.SelectMany(e => e.Pronunciations).ToArray();
                var pronunMultinodes = pronunBatch.Select(p => _pronunciationNodeSerializer.Serialize(p));

                var pronunNodes = await CreateAsync(pronunMultinodes);
                var pronun2id = Enumerable.Range(0, pronunBatch.Length)
                    .ToDictionary(i => pronunBatch[i], i => pronunNodes[i].Id);

                var relationships =
                    from entry in entryBatch
                    from pronun in entry.Pronunciations
                    select new Relationship(
                        originId: entry2id[entry],
                        targetId: pronun2id[pronun],
                        label: RelationshipLabel.HAS);

                await CreateAsync(relationships);
            }
        }

        /// <summary>
        /// Return all word forms.
        /// </summary>
        public async IAsyncEnumerable<WordPhonologyData> GetWordPhonologyData()
        {
            var session = _driver.AsyncSession();
            try
            {
                var query = $@"
                    MATCH (wf:{NodeLabel.WORD_FORM})
                    OPTIONAL MATCH (wf)-[:{RelationshipLabel.HAS}]->(p:{NodeLabel.PRONUNCIATION})
                    WITH wf, collect(p) AS px
                    WHERE size(px) > 0
                    RETURN wf AS wordForm, px AS pronunciations";

                var cursor = await session.RunAsync(query);
                while (await cursor.FetchAsync())
                {
                    var r = cursor.Current;

                    var wordForm = r["wordForm"].As<INode>();
                    var pronuns = r["pronunciations"].As<List<INode>>();

                    yield return new WordPhonologyData(
                        wordForm: _wordFormNodeSerializer.Deserialize(wordForm),
                        pronunciations: pronuns.Select(p =>
                            _pronunciationNodeSerializer.Deserialize(p))
                            .ToArray());
                }
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task CreateWordPhonologyAsync(
            IEnumerable<WordPhonologyData> data)
        {
            var phonSeqDict = new Dictionary<string, long>();

            foreach (var batch in data.Batch(1000))
            {
                // Get all phonetic sequences (words and syllables) in batch that
                // have not already been created.
                //
                var phonSeqs = batch.SelectMany(d => d.Realizations)
                    .Select(r => r.PhoneticWord)
                    .SelectMany(p =>
                        p.Rhymes.Select(r => r.PhoneticSequence)
                        .Concat(p.InnerRhymes.Select(r => r.PhoneticSequence))
                        .Append(p.PhoneticSequence))
                    .Where(s => !phonSeqDict.ContainsKey(s.IpaRepresentation))
                    .Select(s => _phoneticSequenceNodeSerializer.Serialize(s))
                    .ToArray();

                var phonSeqNodes = await CreateAsync(phonSeqs);

                foreach (var node in phonSeqNodes)
                {
                    var phonSeq = _phoneticSequenceNodeSerializer.Deserialize(node);
                    phonSeqDict[phonSeq.IpaRepresentation] = phonSeq.Id.Value;
                }


                var relationships = new List<Relationship>();

                foreach (var item in batch)
                {
                    foreach (var phonReal in item.Realizations)
                    {
                        var phonRealId = phonSeqDict[phonReal.PhoneticWord.PhoneticSequence.IpaRepresentation];
                        var relation = new Relation<PhoneticRealizationRelation>(
                            item.WordForm.Id.Value,
                            phonRealId,
                            new PhoneticRealizationRelation());

                        relationships.Add(_phoneticRealizationRelationSerializer.Serialize(relation));

                        foreach (var rhyme in phonReal.PhoneticWord.Rhymes)
                        {
                            var rhymeId = phonSeqDict[rhyme.PhoneticSequence.IpaRepresentation];
                            var rhymeRel = new Relation<RhymeRelation>(
                                item.WordForm.Id.Value,
                                phonRealId,
                                new RhymeRelation());

                            relationships.Add(_rhymeRelationSerializer.Serialize(rhymeRel));
                        }

                        foreach (var innerRhyme in phonReal.PhoneticWord.InnerRhymes)
                        {
                            var innerRhymeId = phonSeqDict[innerRhyme.PhoneticSequence.IpaRepresentation];
                            var innerRhymeRel = new Relation<InnerRhymeRelation>(
                                item.WordForm.Id.Value,
                                phonRealId,
                                new InnerRhymeRelation());

                            relationships.Add(_innerRhymeRelationSerializer.Serialize(innerRhymeRel));
                        }
                    }
                }

                await CreateAsync(relationships);
            }
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
            var q = $@"CALL apoc.periodic.iterate('MATCH (g:{NodeLabel.GLAFF_ENTRY}) OPTIONAL MATCH (g)-[:{RelationshipLabel.HAS}*]->(p:{NodeLabel.PRONUNCIATION}) RETURN g, p', 'DETACH DELETE g, p', {{batchSize: {batchSize}}})";
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
