using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public class WordForm
    {
        public GlaffEntry GlaffEntry { get; }
        public List<IAnomaly> Anomalies { get; } = new List<IAnomaly>();
        public bool IsValid { get; set; } = true;
        public bool IsLemma { get; set; } = false;
        public string Pronunciation { get; set; }
        public ChangeHistory PronunciationHistory { get; set; }
        public PhoneticRealization StdRealization { get; set; }
        public PhoneticRealization[] Realizations { get; set; } = Array.Empty<PhoneticRealization>();

        public Phoneme[] Phonemes { get; set; }
        public Syllable[] Syllables { get; set; }
        public string Alignment { get; set; }
        //public Rhymes Rhymes { get; set; }

        public WordForm(GlaffEntry glaffEntry)
        {
            GlaffEntry = glaffEntry;
            Pronunciation = glaffEntry.IpaPronunciations;
            PronunciationHistory = new ChangeHistory(Pronunciation);
        }
    }

}
