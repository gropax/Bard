using Bard.Fra.Glaff;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public interface IAnalysisModule
    {
        public void Analyze(WordForm wordForm);
    }

    public class AnalysisPipeline
    {
        private IAnalysisModule[] _modules;

        public AnalysisPipeline(params IAnalysisModule[] modules)
        {
            _modules = modules;
        }

        public WordForm Analyze(GlaffEntry entry)
        {
            var wordForm = new WordForm(entry);

            foreach (var module in _modules)
            {
                module.Analyze(wordForm);

                // Stop further analysis if word is invalid
                if (!wordForm.IsValid)
                    break;
            }

            return wordForm;
        }
    }

    public class WordForm
    {
        public GlaffEntry GlaffEntry { get; }
        public List<IAnomaly> Anomalies { get; } = new List<IAnomaly>();
        public bool IsValid { get; set; } = true;
        public bool IsLemma { get; set; } = false;

        public WordForm(GlaffEntry glaffEntry)
        {
            GlaffEntry = glaffEntry;
        }
    }

    public enum AnomalyType
    {
        NoPhoneme,
        Acronym,
    }

    public interface IAnomaly
    {
        AnomalyType Type { get; }
    }

    public class GenericAnomaly : IAnomaly
    {
        public AnomalyType Type { get; }
        public GenericAnomaly(AnomalyType Type)
        {
            this.Type = Type;
        }
    }
}
