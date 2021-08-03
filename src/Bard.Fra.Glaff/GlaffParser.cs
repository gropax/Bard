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

                    yield return new GlaffEntry()
                    {
                        OldFashioned = false,
                        GraphicalForm = columns[0],
                        MorphoSyntax = columns[1],
                        Lemma = columns[2],
                        ApiPronunciations = columns[3],
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
                        ApiPronunciations = columns.Length > 3 ? columns[3] : null,
                    };
                }
            }
        }
    }
}
