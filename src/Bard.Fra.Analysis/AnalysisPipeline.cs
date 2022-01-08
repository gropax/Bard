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
        public void Analyze(WordForm word);
    }

    public class AnalysisPipeline
    {
        private IAnalysisModule[] _modules;

        public AnalysisPipeline(params IAnalysisModule[] modules)
        {
            _modules = modules;
        }

        public MultiNode Analyze(GlaffEntry entry)
        {
            var wordForm = new WordForm(entry);

            foreach (var module in _modules)
                module.Analyze(wordForm);

            var wordFormNode = new WordFormNodeType(wordForm.GlaffEntry.GraphicalForm);

            return new MultiNode(wordFormNode);
        }
    }

    public class WordForm
    {
        public GlaffEntry GlaffEntry { get; }

        public WordForm(GlaffEntry glaffEntry)
        {
            GlaffEntry = glaffEntry;
        }
    }
}
