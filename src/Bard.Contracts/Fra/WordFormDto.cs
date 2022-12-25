using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public class WordFormDto
    {
        public long Id { get; }
        public string Graphemes { get; }
        public string Syllables { get; }
        public POS Pos { get; }
        public Number? Number { get; }
        public Gender? Gender { get; }
        public Person? Person { get; }
        public Mood? Mood { get; }
        public Tense? Tense { get; }

        public WordFormDto(
            long id,
            string graphemes,
            string syllables,
            POS pos,
            Number? number = null,
            Gender? gender = null,
            Person? person = null,
            Mood? mood = null,
            Tense? tense = null)
        {
            Id = id;
            Graphemes = graphemes;
            Syllables = syllables;
            Pos = pos;
            Number = number;
            Gender = gender;
            Person = person;
            Mood = mood;
            Tense = tense;
        }
    }

    public class PhonGraphWordDto
    {
        public string Graphemes { get; }
        public string Syllables { get; }

        public PhonGraphWordDto(string graphemes, string syllables)
        {
            Graphemes = graphemes;
            Syllables = syllables;
        }
    }

    public class WordPronunciation
    {
        public string Graphemes { get; }
        public POS POS { get; }
        public string Ipa { get; }

        //public string Syllables { get; }

        public WordPronunciation(string graphemes, POS pos, string ipa)
        {
            Graphemes = graphemes;
            POS = pos;
            Ipa = ipa;
        }
    }
}
