using Bard.Contracts.Fra;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Glaff
{
    public static class GlaffParser
    {
        public static IEnumerable<GlaffEntry> ParseMainLexicon(string path)
        {
            const int BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize)) {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] columns = line.Split('|');

                    string morphoSyntax = columns[1];
                    ParseGRACE(morphoSyntax, out var pos, out var gender, out var number, out var person, out var mood, out var tense);

                    yield return new GlaffEntry()
                    {
                        OldFashioned = false,

                        Lemma = columns[2],
                        GraphicalForm = columns[0],

                        MorphoSyntax = morphoSyntax,
                        POS = pos,
                        Person = person,
                        Gender = gender,
                        Number = number,
                        Mood = mood,
                        Tense = tense,

                        IpaPronunciations = columns[3],
                        SampaPronunciations = columns[4],

                        FrantexAbsoluteFormFrequency = ParseDouble(columns[5]),
                        FrantexRelativeFormFrequency = ParseDouble(columns[6]),
                        FrantexAbsoluteLemmaFrequency = ParseDouble(columns[7]),
                        FrantexRelativeLemmaFrequency = ParseDouble(columns[8]),
                        LM10AbsoluteFormFrequency = ParseDouble(columns[9]),
                        LM10RelativeFormFrequency = ParseDouble(columns[10]),
                        LM10AbsoluteLemmaFrequency = ParseDouble(columns[11]),
                        LM10RelativeLemmaFrequency = ParseDouble(columns[12]),
                        FrWacAbsoluteFormFrequency = ParseDouble(columns[13]),
                        FrWacRelativeFormFrequency = ParseDouble(columns[14]),
                        FrWacAbsoluteLemmaFrequency = ParseDouble(columns[15]),
                        FrWacRelativeLemmaFrequency = ParseDouble(columns[16]),
                    };
                }
            }
        }

        private static double ParseDouble(string str) => double.Parse(str, CultureInfo.InvariantCulture);

        public static IEnumerable<GlaffEntry> ParseOldiesLexicon(string path)
        {
            const int BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize)) {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] columns = line.Split('|');

                    yield return new GlaffEntry()
                    {
                        OldFashioned = true,
                        GraphicalForm = columns[0],
                        MorphoSyntax = columns[1],
                        Lemma = columns[2],
                        IpaPronunciations = columns.Length > 3 ? columns[3] : null,
                    };
                }
            }
        }


        private static void ParseGRACE(string traits, out POS pos, out Gender? gender, out Number? number, out Person? person, out Mood? mood, out Tense? tense)
        {
            char fst = traits[0];
            gender = null;
            number = null;
            person = null;
            mood = null;
            tense = null;

            switch (fst)
            {
                case 'A':
                    pos = POS.Adjective;
                    gender = ParseGender(traits[3]);
                    number = ParseNumber(traits[4]);
                    return;

                case 'R':
                    pos = POS.Adverb;
                    return;

                case 'C':
                    pos = POS.Conjunction;
                    return;

                case 'D':
                    pos = POS.Determiner;
                    person = ParsePerson(traits[2]);
                    gender = ParseGender(traits[3]);
                    number = ParseNumber(traits[4]);
                    return;

                case 'I':
                    pos = POS.Interjection;
                    return;

                case 'N':
                    pos = POS.Noun;
                    gender = ParseGender(traits[2]);
                    number = ParseNumber(traits[3]);
                    return;

                case 'P':
                    pos = POS.Pronoun;
                    person = ParsePerson(traits[2]);
                    gender = ParseGender(traits[3]);
                    number = ParseNumber(traits[4]);
                    return;

                case 'S':
                    pos = POS.Preposition;
                    return;

                case 'V':
                    pos = POS.Verb;
                    (mood, tense) = ParseMoodAndTense(traits[2], traits[3]);
                    person = ParsePerson(traits[4]);
                    number = ParseNumber(traits[5]);
                    gender = ParseGender(traits[6]);
                    return;

                default:
                    throw new Exception($"Unexpected POS value [{fst}].");
            }
        }

        private static Gender ParseGender(char c) => c == 'm' ? Gender.Masculine : Gender.Feminine; 
        private static Number ParseNumber(char c) => c == 's' ? Number.Singular : Number.Plural; 
        private static Person ParsePerson(char s)
        {
            switch (s)
            {
                case '-': return Person.Indeterminate;
                case '1': return Person.First;
                case '2': return Person.Second;
                case '3': return Person.Third;
                default:
                    throw new Exception($"Unexpected person value [{s}].");
            }
        }
        private static (Mood, Tense) ParseMoodAndTense(char m, char t)
        {
            if (m == 'n')
                return (Mood.Infinitive, Tense.Infinitive);

            else if (m == 'p')
            {
                var mood = Mood.Participle;
                var tense = t == 'p' ? Tense.PresentParticiple : Tense.PastParticiple;
                return (mood, tense);
            }

            else if (m == 'i')
            {
                var mood = Mood.Indicative;
                if (t == 'p')
                    return (mood, Tense.Present);
                else if (t == 'i')
                    return (mood, Tense.Imperfect);
                else if (t == 'f')
                    return (mood, Tense.Futur);
                else if (t == 's')
                    return (mood, Tense.Futur);
            }

            else if (m == 'c')
                return (Mood.Conditional, Tense.Conditional);

            else if (m == 'm')
                return (Mood.Imperative, Tense.Imperative);

            else if (m == 's')
            {
                if (t == 'p')
                    return (Mood.Subjunctive, Tense.PresentSubjunctive);
                else if (t == 'i')
                    return (Mood.Subjunctive, Tense.ImperfectSubjunctive);
            }

            throw new Exception($"Unexpected mood/tense combination [{m}, {t}].");
        }
    }
}
