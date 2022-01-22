using Bard.Fra.Glaff;
using Intervals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xunit;

namespace Bard.Fra.Analysis.Phonology.Tests
{
    public class NaivePhonologicalAlignerTests : TestBase
    {
        [Theory]
        [InlineData("boutique", "b:b ou:u t:t i:i qu:k e:")]
        [InlineData("anticonstitutionnellement", "an:@ t:t i:i c:k on:§ s:s t:t i:i t:t u:y t:s i:j o:o nn:n e:E ll:l e:° m:m en:@ t:")]

        // /p/ sound
        [InlineData("pomme", "p:p o:O mm:m e:")]
        [InlineData("frappe", "f:f r:R a:a pp:p e:")]
        [InlineData("pha", "ph:p a:a")]
        [InlineData("ppha", "pph:p a:a")]

        // /t/ sound
        [InlineData("taupe", "t:t au:o p:p e:")]
        [InlineData("sculpter", "s:s c:k u:y l:l pt:t e:e r:")]
        [InlineData("thé", "th:t é:e")]
        [InlineData("matthéen", "m:m a:a tth:t é:e en:5")]

        // /k/ sound
        [InlineData("cadeaux", "c:k a:a d:d eau:o x:")]
        [InlineData("accroc", "a:a cc:k r:R o:o c:")]
        [InlineData("cinq", "c:s in:5 q:k")]
        [InlineData("qatar", "q:k a:a t:t a:a r:R")]
        [InlineData("quinze", "qu:k in:5 z:z e:")]
        [InlineData("chianti", "ch:k i:i an:@ t:t i:i")]
        [InlineData("vecchio", "v:v e:e cch:k i:i o:o")]
        [InlineData("akène", "a:a k:k è:E n:n e:")]
        [InlineData("trekking", "t:t r:R e:E kk:k i:i ng:G")]
        [InlineData("cheikh", "ch:S ei:E kh:k")]
        [InlineData("kha", "kh:k a:a")]
        [InlineData("kkha", "kkh:k a:a")]

        // /b/ sound
        [InlineData("bourde", "b:b ou:u r:R d:d e:")]
        [InlineData("abbé", "a:a bb:b é:e")]
        [InlineData("abhorré", "a:a bh:b o:o rr:R é:e")]
        [InlineData("bbha", "bbh:b a:a")]

        // /d/ sound
        [InlineData("dur", "d:d u:y r:R")]
        [InlineData("addict", "a:a dd:d i:i c:k t:t")]
        [InlineData("bouddha", "b:b ou:u ddh:d a:a")]
        [InlineData("dharma", "dh:d a:a r:R m:m a:a")]

        // /g/ sound
        [InlineData("gâteau", "g:g â:a t:t eau:o")]
        [InlineData("guichet", "gu:g i:i ch:S e:E t:")]
        [InlineData("ghana", "gh:g a:a n:n a:a")]
        [InlineData("agglomérat", "a:a gg:g l:l o:o m:m é:e r:R a:a t:")]
        [InlineData("aggha", "a:a ggh:g a:a")]

        // /f/ sound
        [InlineData("faon", "f:f aon:@")]
        [InlineData("affaire", "a:a ff:f ai:E r:R e:")]
        [InlineData("philosophe", "ph:f i:i l:l o:o s:z o:O ph:f e:")]

        // /s/ sound
        [InlineData("cerise", "c:s e:° r:R i:i s:z e:")]
        [InlineData("sauver", "s:s au:o v:v e:e r:")]
        [InlineData("casser", "c:k a:a ss:s e:e r:")]
        [InlineData("garçon", "g:g a:a r:R ç:s on:§")]
        [InlineData("faction", "f:f a:a c:k t:s i:j on:§")]
        [InlineData("auxerre", "au:o x:s e:E rr:R e:")]

        // /ʃ/ sound
        [InlineData("charrue", "ch:S a:a rr:R u:y e:")]
        [InlineData("shah", "sh:S a:a h:")]

        // /v/ sound
        [InlineData("voiture", "v:v oi:wa t:t u:y r:R e:")]
        [InlineData("wagon", "w:v a:a g:g on:§")]

        // /z/ sound
        [InlineData("rose", "r:R o:o s:z e:")]
        [InlineData("zoé", "z:z o:o é:e")]
        [InlineData("blizzard", "b:b l:l i:i zz:z a:a r:R d:")]

        // /ʒ/ sound
        [InlineData("géant", "g:Z é:e an:@ t:")]
        [InlineData("juri", "j:Z u:y r:R i:i")]
        [InlineData("rangeai", "r:R an:@ ge:Z ai:e")]
        [InlineData("pigeon", "p:p i:i ge:Z on:§")]
        [InlineData("gageüre", "g:g a:a ge:Z ü:y r:R e:")]

        // /l/ sound
        [InlineData("livre", "l:l i:i v:v r:R e:")]
        [InlineData("allez", "a:a ll:l e:e z:")]

        // /r/ sound
        [InlineData("rue", "r:R u:y e:")]
        [InlineData("barre", "b:b a:a rr:R e:")]

        // /m/ sound
        [InlineData("monde", "m:m on:§ d:d e:")]
        [InlineData("immonde", "i:i mm:m on:§ d:d e:")]

        // /n/ sound
        [InlineData("ananas", "a:a n:n a:a n:n a:a s:s")]
        [InlineData("donne", "d:d o:o nn:n e:")]
        [InlineData("damner", "d:d a:a mn:n e:e r:")]

        // /ɲ/ sound
        [InlineData("peigne", "p:p ei:E gn:N e:")]

        // /ŋ/ sound
        [InlineData("parking", "p:p a:a r:R k:k i:i ng:G")]

        // /j/ sound
        [InlineData("faille", "f:f a:a ill:j e:")]
        [InlineData("travail", "t:t r:R a:a v:v a:a il:j")]
        [InlineData("fion", "f:f i:j on:§")]
        [InlineData("rayon", "r:R a:E y:j on:§")]
        [InlineData("aja", "a:a j:j a:a")]
        [InlineData("ïambique", "ï:j am:@ b:b i:i qu:k e:")]

        // /w/ sound ", new string[] { "ou", "ww", "wh", "w" }},
        [InlineData("oui", "ou:w i:i")]
        [InlineData("awa", "a:a w:w a:a")]

        // /ɥ/ sound
        [InlineData("huit", "h: u:8 i:i t:t")]

        // /i/ sound
        [InlineData("ville", "v:v i:i ll:l e:")]
        [InlineData("yves", "y:i v:v es:")]
        [InlineData("naïf", "n:n a:a ï:i f:f")]
        [InlineData("île", "î:i l:l e:")]

        // /e/ sound
        [InlineData("mangeai", "m:m an:@ ge:Z ai:e")]  // prononciation non standard
        [InlineData("créées", "c:k r:R é:e é:e es:")]
        [InlineData("aesophage", "ae:e s:z o:o ph:f a:a g:Z e:")]
        [InlineData("æsophage", "æ:e s:z o:o ph:f a:a g:Z e:")]
        [InlineData("oedipe", "oe:e d:d i:i p:p e:")]
        [InlineData("œdipe", "œ:e d:d i:i p:p e:")]
        [InlineData("aî", "aî:e")]  // ???
        [InlineData("e", "e:e")] // ???

        // /ɛ/ sound
        [InlineData("sortai", "s:s o:O r:R t:t ai:E")]
        [InlineData("affaîter", "a:a ff:f aî:E t:t e:e r:")]
        [InlineData("abeille", "a:a b:b e:E ill:j e:")]
        [InlineData("blette", "b:b l:l e:E tt:t e:")]
        [InlineData("près", "p:p r:R è:E s:")]
        [InlineData("fête", "f:f ê:E t:t e:")]
        [InlineData("noël", "n:n o:o ë:E l:l")]
        [InlineData("aéroport", "a:a é:E r:R o:o p:p o:O r:R t:")] // Prononciation non standard
        [InlineData("aesophage", "ae:E s:z o:o ph:f a:a g:Z e:")] // Prononciation non standard
        [InlineData("æsophage", "æ:E s:z o:o ph:f a:a g:Z e:")] // Prononciation non standard

        // /a/ sound
        [InlineData("aaron", "aa:a r:R on:§")]
        [InlineData("vache", "v:v a:a ch:S e:")]
        [InlineData("château", "ch:S â:a t:t eau:o")]

        // /ɑ/ sound
        [InlineData("gras", "g:g r:R a:A s:")]
        [InlineData("pâtes", "p:p â:A t:t es:")]

        // /ɔ/ sound
        [InlineData("porc", "p:p o:O r:R c:")]

        // /o/ sound
        [InlineData("rot", "r:R o:o t:")]
        [InlineData("chaud", "ch:S au:o d:")]
        [InlineData("contrôle", "c:k on:§ t:t r:R ô:o l:l e:")]
        [InlineData("bateau", "b:b a:a t:t eau:o")]

        // /u/ sound
        [InlineData("roue", "r:R ou:u e:")]
        [InlineData("où", "où:u")]
        [InlineData("août", "a:a oû:u t:")]
        [InlineData("tutti", "t:t u:u tt:t i:i")]

        // /y/ sound
        [InlineData("bulle", "b:b u:y ll:l e:")]
        [InlineData("mûre", "m:m û:y r:R e:")]
        [InlineData("contigüe", "c:k on:§ t:t i:i g:g ü:y e:")]

        // /ø/ sound
        [InlineData("deux", "d:d eu:2 x:")]
        [InlineData("oeufs", "oeu:2 fs:")]
        [InlineData("œufs", "œu:2 fs:")]
        [InlineData("oedipe", "oe:2 d:d i:i p:p e:")]  // Prononciation non standard
        [InlineData("œdipe", "œ:2 d:d i:i p:p e:")]   // Prononciation non standard

        // /œ/ sound
        [InlineData("beurre", "b:b eu:9 rr:R e:")]
        [InlineData("oeuf", "oeu:9 f:f")]
        [InlineData("œuf", "œu:9 f:f")]
        [InlineData("über", "ü:y b:b e:9 r:R")]
        [InlineData("oedipe", "oe:9 d:d i:i p:p e:")]  // Prononciation non standard
        [InlineData("œdipe", "œ:9 d:d i:i p:p e:")]   // Prononciation non standard

        // /ə/ sound
        [InlineData("lâchement", "l:l â:a ch:S e:° m:m en:@ t:")]

        // /ɛ̃/ sound
        [InlineData("alain", "a:a l:l ain:5")]
        [InlineData("klein", "k:k l:l ein:5")]
        [InlineData("brin", "b:b r:R in:5")]
        [InlineData("chien", "ch:S i:j en:5")]

        // /ɑ̃/ sound
        [InlineData("taon", "t:t aon:@")]
        [InlineData("flan", "f:f l:l an:@")]
        [InlineData("chambre", "ch:S am:@ b:b r:R e:")]
        [InlineData("enfermé", "en:@ f:f e:E r:R m:m é:e")]
        [InlineData("camembert", "c:k a:a m:m em:@ b:b e:E r:R t:")]

        // /ɔ̃/ sound
        [InlineData("jambon", "j:Z am:@ b:b on:§")]
        [InlineData("combe", "c:k om:§ b:b e:")]

        // /œ̃/ sound
        [InlineData("brun", "b:b r:R un:1")]
        [InlineData("parfum", "p:p a:a r:R f:f um:1")]

        // Silent endings
        [InlineData("cornet", "c:k o:O r:R n:n e:E t:")]
        [InlineData("manger", "m:m an:@ g:Z e:e r:")]
        [InlineData("mange", "m:m an:@ g:Z e:")]
        [InlineData("manges", "m:m an:@ g:Z es:")]
        [InlineData("mangent", "m:m an:@ g:Z ent:")]
        [InlineData("jonc", "j:Z on:§ c:")]
        [InlineData("plomb", "p:p l:l om:§ b:")]
        [InlineData("lard", "l:l a:a r:R d:")]
        [InlineData("clef", "c:k l:l e:e f:")]

        // Silent 'h'
        [InlineData("hôpital", "h: ô:o p:p i:i t:t a:a l:l")]  // 'h' silent at beginning
        [InlineData("dehors", "d:d e:° h: o:O r:R s:")]        // 'h' silent inside
        [InlineData("poussah", "p:p ou:u ss:s a:a h:")]        // 'h' silent at the end

        // Special case
        [InlineData("sprezzatura", "s:s p:p r:R e:e zz:tz a:a t:t u:u r:R a:a")]
        [InlineData("poire", "p:p oi:wa r:R e:")]
        [InlineData("point", "p:p oin:w5 t:")]
        [InlineData("maxime", "m:m a:a x:ks i:i m:m e:")]
        [InlineData("xavier", "x:gz a:a v:v i:j e:e r:")]
        [InlineData("max", "m:m a:a x:ks")]
        public void TestCompute(string graphemes, string rawAlignement)
        {
            var alignment = ParseAlignement(rawAlignement).ToArray();
            var phonemes = alignment.SelectMany(a => a.phonemes).ToArray();

            var aligner = new NaivePhonologicalAligner(graphemes, phonemes);
            var result = aligner.Compute();
            var trace = aligner.GetTrace();

            Assert.NotNull(result);
            Assert.Equal(alignment.Length, result.Length);

            for (int i = 0; i < result.Length; i++)
            {
                var realInterval = result[i];
                var realGraphemes = graphemes.Substring(realInterval.Start, realInterval.Length);
                var realPhonemes = string.Join("", realInterval.Value);

                var expectedInterval = alignment[i];

                Assert.Equal(expectedInterval.graphemes, realGraphemes);
                Assert.Equal(string.Join("", expectedInterval.phonemes), realPhonemes);
            }
        }

        private IEnumerable<(string graphemes, string[] phonemes)> ParseAlignement(string rawAlignment)
        {
            foreach (var interval in rawAlignment.Split(' '))
            {
                var parts = interval.Split(':');
                var graphemes = parts[0];
                var phonemes = ParsePhonemes(parts[1]);

                yield return (graphemes, phonemes);
            }
        }
    }
}
