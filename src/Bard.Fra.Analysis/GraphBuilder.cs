﻿using Bard.Contracts.Fra;
using Bard.Fra.Analysis.Phonology;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public class GraphBuilder
    {
        private GraphStorage _storage;
        private MultiNodeBuilder _nodeBuilder = new MultiNodeBuilder();
        public GraphBuilder(GraphStorage storage)
        {
            _storage = storage;
        }

        private Dictionary<Tuple<string, POS>, long> _lemmaIdMapping = new Dictionary<Tuple<string, POS>, long>();
        private List<LemmaRelation> _pendingLemmaRels = new List<LemmaRelation>();
        private Dictionary<string, long> _phonSeqIdMapping = new Dictionary<string, long>();

        public async Task ProcessAsync(IEnumerable<WordForm> wordFormStream)
        {
            var wordForms = wordFormStream.ToArray();

            // Build new WordForm nodes
            var wordFormMultiNodes = wordForms.Select(wf => _nodeBuilder.Build(wf)).ToArray();

            // Store WordForm nodes, create ID mappings
            var wordFormNodes = await _storage.CreateAsync(wordFormMultiNodes);
            var rankField = _nodeBuilder.Format(WordFormField.Rank);
            var wordFormIdMapping = wordFormNodes.ToDictionary(
                n => n[rankField].As<int>(),
                n => n.Id);


            // Collect all PhonSeqs
            var wfPhonSeqs = wordForms
                .SelectMany(wf => wf.Realizations)
                .Select(r => r.PhoneticSequence);

            var phonSeqs = new HashSet<PhoneticSequence>(wfPhonSeqs, new PhoneticSequenceIdComparer());

            // Build new PhonSeq nodes
            var newPhonSeqs = phonSeqs.Where(s => !_phonSeqIdMapping.ContainsKey(s.Id)).ToArray();
            var phonSeqMultinodes = newPhonSeqs.Select(s => _nodeBuilder.Build(s)).ToArray();

            // Store new PhonSeq nodes, update ID mappings
            var phonSeqNodes = await _storage.CreateAsync(phonSeqMultinodes);
            var phonemesField = _nodeBuilder.Format(PhoneticSequenceField.Phonemes);
            foreach (var n in phonSeqNodes)
                _phonSeqIdMapping[n[phonemesField].As<string>()] = n.Id;


            // Collect all relations
            //
            var lemmaRels = new List<LemmaRelation>();
            var phonRealRels = new List<PhoneticRealizationRelation>();

            foreach (var wordForm in wordForms)
            {
                if (wordForm.IsLemma)
                    _lemmaIdMapping[Tuple.Create(wordForm.GlaffEntry.Lemma, wordForm.GlaffEntry.POS)] = wordFormIdMapping[wordForm.GlaffEntry.Rank];

                lemmaRels.Add(new LemmaRelation(
                    wordFormNodeId: wordFormIdMapping[wordForm.GlaffEntry.Rank],
                    lemma: wordForm.GlaffEntry.Lemma,
                    pos: wordForm.GlaffEntry.POS));

                foreach (var real in wordForm.Realizations)
                {
                    long wordFormNodeId = wordFormIdMapping[wordForm.GlaffEntry.Rank];
                    long phonSeqNodeId = _phonSeqIdMapping[real.PhoneticSequence.Id];
                    phonRealRels.Add(new PhoneticRealizationRelation(wordFormNodeId, phonSeqNodeId, real));
                }
            }

            // Store PhonReal relationships
            var phonRealRelationships = phonRealRels
                .Select(r => _nodeBuilder.Build(
                    originId: r.WordFormNodeId,
                    targetId: r.PhonSeqNodeId,
                    realization: r.Realization));

            await _storage.CreateAsync(phonRealRelationships);

            // Build LEMMA relationships if Lemma node already exists, otherwise store for later try
            var pendingLemmaRels = new List<LemmaRelation>();
            var lemmaRelationships = new List<Relationship>();

            foreach (var rel in _pendingLemmaRels.Concat(lemmaRels))
            {
                if (_lemmaIdMapping.TryGetValue(rel.LemmaId, out var lemmaId))
                    lemmaRelationships.Add(_nodeBuilder.BuildLemmaRelation(
                        originId: rel.WordFormNodeId,
                        targetId: lemmaId));
                else
                    pendingLemmaRels.Add(rel);
            }

            _pendingLemmaRels = pendingLemmaRels;

            await _storage.CreateAsync(lemmaRelationships);
        }
    }


    public class LemmaRelation
    {
        public long WordFormNodeId { get; } 
        public Tuple<string, POS> LemmaId { get; }

        public LemmaRelation(long wordFormNodeId, string lemma, POS pos)
        {
            WordFormNodeId = wordFormNodeId;
            LemmaId = Tuple.Create(lemma, pos);
        }
    }

    public class PhoneticRealizationRelation
    {
        public long WordFormNodeId { get; } 
        public long PhonSeqNodeId { get; } 
        public PhoneticRealization Realization { get; }

        public PhoneticRealizationRelation(long wordFormNodeId, long phonSeqNodeId, PhoneticRealization realization)
        {
            WordFormNodeId = wordFormNodeId;
            PhonSeqNodeId = phonSeqNodeId;
            Realization = realization;
        }
    }
}
