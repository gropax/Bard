using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public class WordForm
    {
        public string Graphemes { get; }
        public string Syllables { get; }

        public WordForm(string graphemes, string syllables)
        {
            Graphemes = graphemes;
            Syllables = syllables;
        }
    }
}
