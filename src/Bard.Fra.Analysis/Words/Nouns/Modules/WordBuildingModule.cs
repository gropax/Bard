using Bard.Contracts.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis.Words.Nouns.Modules
{
    public class WordBuildingConfig
    {
        public bool Enabled { get; set; } = true;
    }

    public class WordBuildingModule : IAnalysisModule<LemmaData<NounLemma, NounForm>>
    {
        public WordBuildingConfig Config { get; }

        public WordBuildingModule(WordBuildingConfig config)
        {
            Config = config;
        }

        public bool Analyze(AnalysisResult<LemmaData<NounLemma, NounForm>> result)
        {
            var lemmaData = result.Result;

            foreach (var wordFormData in lemmaData.WordForms)
            {
                var bestGlaff = wordFormData.GlaffEntries
                    .OrderByDescending(g => g.FrantexAbsoluteFormFrequency)
                    //.ThenBy(g => g.Pronunciations.Min(p => p.Anomalies))
                    .First();

                var pronunciations = bestGlaff.Pronunciations
                    .Where(p => p.Anomalies.Count() == 0)
                    .ToArray();

                wordFormData.WordForm = new NounForm()
                {
                    Lemma = bestGlaff.Lemma,
                    GraphicalForm = bestGlaff.GraphicalForm,
                    Gender = bestGlaff.Gender,
                    Number = bestGlaff.Number,
                };
                wordFormData.Pronunciations = pronunciations;
            }

            var fstForm = lemmaData.WordForms
                .Select(d => d.WordForm)
                .OrderBy(f => f.Number)  // pick singular as lemma label, if available
                .First();

            lemmaData.Lemma = new NounLemma()
            {
                GraphicalForm = fstForm.GraphicalForm,
                Gender = fstForm.Gender,
            };

            bool abort = false;
            return abort;
        }
    }
}
