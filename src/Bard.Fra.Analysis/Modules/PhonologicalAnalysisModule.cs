using Bard.Fra.Analysis.Phonology;
using Intervals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public static class PhonologicalAnalysisModuleFactory
    {
        public class Config
        {
            public bool Enabled { get; set; } = true;
        }

        public static PhonologicalAnalysisModule Build(Config config)
        {
            return new PhonologicalAnalysisModule();
        }
    }

    public class PhonologicalAnalysisModule : IAnalysisModule
    {
        public void Analyze(WordForm wordForm)
        {
            if (string.IsNullOrWhiteSpace(wordForm.Pronunciation))
                return;

            var graphemes = wordForm.GlaffEntry.GraphicalForm;

            wordForm.Syllables = new Syllabifier().Compute(wordForm.Phonemes).ToArray();
        }
    }
}
