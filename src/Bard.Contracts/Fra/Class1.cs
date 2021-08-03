using System;

namespace Bard.Contracts.Fra
{
    public class Lemma
    {
        public string WrittenForm { get; }
        public POS POS { get; }
    }

    public class WordForm
    {
        public string Lemma { get; set; }
        public string WrittenForm { get; set; }

        // Grammatical traits

        public POS POS { get; set; }



        /// <summary>
        /// The phonological word starts with either a consonant or an « aspirated h » (neither a vowel or semi-vowel)
        /// </summary>
        public bool StartsWithConsonant { get; set; }

        /// <summary>
        /// The word ends with an elidable schwa (elision reduce the syllable count by 1)
        /// </summary>
        public bool IsElidable { get; set; }

        /// <summary>
        /// Number of possible synaeresis (each synaeresis reduce the syllable count by 1)
        /// </summary>
        public int PossibleSynaeresis { get; set; }

        /// <summary>
        /// Number of possible diaeresis (each diaeresis increase the syllable count by 1)
        /// </summary>
        public int PossibleDiaeresis { get; set; }

        // Syllabic counts

        /// <summary>
        /// Standard number of syllables (as when pronounced alone)
        /// </summary>
        public int Syllables { get; set; }

        // Rhyme

        /// <summary>
        /// Rhyme in the form: VC*$
        /// </summary>
        public string Rhyme1 { get; set; }

        /// <summary>
        /// Rhyme in the form: CVC*$
        /// </summary>
        public string Rhyme2 { get; set; }

        /// <summary>
        /// Rhyme in the form: VC*VC*$
        /// </summary>
        public string Rhyme3 { get; set; }

        /// <summary>
        /// Rhyme in the form: C*VC*VC*$
        /// </summary>
        public string Rhyme4 { get; set; }

        /// <summary>
        /// Gender of the rhyme
        /// </summary>
        public Gender RhymeGender { get; set; }

        /// <summary>
        /// In the context of classical french metric, words rhyme with other words of the same gender
        /// but also with the same « consonant ending » (can be either 's', 't', 'ent', or '')
        /// </summary>
        public string RhymeSubclass { get; set; }
    }

    public enum POS
    {
        Adjective,
        Adverb,
        Conjunction,
        Determiner,
        Interjection,
        Noun,
        Pronoun,
        Preposition,
        Verb,
    }

    public enum Gender
    {
        Masculine,
        Feminine,
    }

    public enum Number
    {
        Singular,
        Plural,
    }

    public enum Person
    {
        First,
        Second,
        Third,
    }

    public enum Tense
    {
    }

    public enum Mood
    {
        Indicative,
        Subjunctive,
        Conditional,
        Imperative,
        Infinitive,
        PastParticiple,
    }
}
