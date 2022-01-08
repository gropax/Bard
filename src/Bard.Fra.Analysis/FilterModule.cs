using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public static class FilterModuleFactory
    {
        public class Config
        {
            public bool Enabled { get; set; } = true;
            public bool FilterAcronyms { get; set; } = true;
            public bool FilterWithoutPhonemes { get; set; } = true;
        }

        public static FilterModule Build(Config config)
        {
            var filters = new List<WordFormFilter>();

            if (config.FilterWithoutPhonemes)
                filters.Add(new WordFormFilter("WithoutPhonemes",
                    (wordForm) => string.IsNullOrWhiteSpace(wordForm.GlaffEntry.IpaPronunciations)));

            if (config.FilterAcronyms)
                filters.Add(new WordFormFilter("Acronym", (wordForm) =>
                {
                    var graphemes = wordForm.GlaffEntry.GraphicalForm;
                    return graphemes.Length > 2 && Char.IsUpper(graphemes[0]) && Char.IsUpper(graphemes[1]);
                }));

            return new FilterModule(filters.ToArray());
        }
    }

    public class WordFormFilter
    {
        public string Name { get; }
        public Func<WordForm, bool> Condition { get; }
        public WordFormFilter(string name, Func<WordForm, bool> condition)
        {
            Name = name;
            Condition = condition;
        }
    }

    public class FilterModule : IAnalysisModule
    {
        private WordFormFilter[] _filters;

        public FilterModule(WordFormFilter[] filters)
        {
            _filters = filters;
        }

        public void Analyze(WordForm wordForm)
        {
            foreach (var filter in _filters)
            {
                if (filter.Condition(wordForm))
                {
                    wordForm.IsValid = false;
                    return;
                }
            }
        }
    }
}
