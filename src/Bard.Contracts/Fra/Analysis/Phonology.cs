using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra.Analysis.Phonology
{
    public class WordPhonologyData
    {
        public WordForm2 WordForm { get; }
        public Pronunciation[] Pronunciations { get; }

        public List<IAnomaly> Anomalies { get; } = new List<IAnomaly>();
        public string Pronunciation { get; set; }
        public ChangeHistory PronunciationHistory { get; set; }
        public PhoneticRealization MainRealization { get; set; }
        public PhoneticRealization[] Realizations { get; set; } = Array.Empty<PhoneticRealization>();

        public Phoneme[] Phonemes { get; set; }
        public Syllable[] Syllables { get; set; }
        public string Alignment { get; set; }

        public WordPhonologyData(WordForm2 wordForm, Pronunciation[] pronunciations)
        {
            WordForm = wordForm;
            Pronunciations = pronunciations;
        }
    }

}
