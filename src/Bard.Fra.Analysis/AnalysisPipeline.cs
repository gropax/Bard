using Bard.Fra.Analysis.Phonology;
using Bard.Fra.Analysis.Glaff;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bard.Contracts.Fra;

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
}
