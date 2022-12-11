using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Storage.Neo4j.Fra
{
    public static class PropLabel
    {
        public const string GLAFF_RANK = "glaff.raw.rank";
        public const string GLAFF_IS_OLD_FASHIONED = "glaff.raw.is_old_fashioned";
        public const string GLAFF_GRAPHICAL_FORM = "glaff.raw.graphical_form";
        public const string GLAFF_MORPHO_SYNTAX = "glaff.raw.morpho_syntax";
        public const string GLAFF_POS = "glaff.raw.pos";
        public const string GLAFF_PERSON = "glaff.raw.person";
        public const string GLAFF_GENDER = "glaff.raw.gender";
        public const string GLAFF_NUMBER = "glaff.raw.number";
        public const string GLAFF_MOOD = "glaff.raw.mood";
        public const string GLAFF_TENSE = "glaff.raw.tense";
        public const string GLAFF_LEMMA = "glaff.raw.lemma";
        public const string GLAFF_IPA_PRONUNCIATIONS = "glaff.raw.ipa_pronunciations";
        public const string GLAFF_SAMPA_PRONUNCIATIONS = "glaff.raw.sampa_pronunciations";
        public const string GLAFF_FRANTEX_ABS_FORM_FREQ = "glaff.raw.frantex_abs_form_freq";
        public const string GLAFF_FRANTEX_REL_FORM_FREQ = "glaff.raw.frantex_rel_form_freq";
        public const string GLAFF_FRANTEX_ABS_LEMMA_FREQ = "glaff.raw.frantex_abs_lemma_freq";
        public const string GLAFF_FRANTEX_REL_LEMMA_FREQ = "glaff.raw.frantex_rel_lemma_freq";
        public const string GLAFF_LM10_ABS_FORM_FREQ = "glaff.raw.lm10_abs_form_freq";
        public const string GLAFF_LM10_REL_FORM_FREQ = "glaff.raw.lm10_rel_form_freq";
        public const string GLAFF_LM10_ABS_LEMMA_FREQ = "glaff.raw.lm10_abs_lemma_freq";
        public const string GLAFF_LM10_REL_LEMMA_FREQ = "glaff.raw.lm10_rel_lemma_freq";
        public const string GLAFF_FRWAC_ABS_FORM_FREQ = "glaff.raw.frwac_abs_form_freq";
        public const string GLAFF_FRWAC_REL_FORM_FREQ = "glaff.raw.frwac_rel_form_freq";
        public const string GLAFF_FRWAC_ABS_LEMMA_FREQ = "glaff.raw.frwac_abs_lemma_freq";
        public const string GLAFF_FRWAC_REL_LEMMA_FREQ = "glaff.raw.frwac_rel_lemma_freq";
        public const string GLAFF_MISSING_PRONUNCIATION = "glaff.calc.missing_pronunciation";
        public const string GLAFF_IS_ACRONYM = "glaff.calc.is_acronym";
        public const string GLAFF_IS_LEMMA = "glaff.calc.is_lemma";

        public const string PRONUN_GRAPHICAL_FORM = "pronun.raw.graphical_form";
        public const string PRONUN_ORIGINAL_VALUE = "pronun.raw.original_value";
        public const string PRONUN_FINAL_VALUE = "pronun.calc.final_value";
        public const string PRONUN_CHANGE_HISTORY = "pronun.calc.change_history";
        public const string PRONUN_PHONEMES = "pronun.calc.phonemes";
        public const string PRONUN_ALIGNMENT = "pronun.calc.alignment";
        public const string PRONUN_ANOMALY_COUNT = "pronun.calc.anomaly_count";
        public const string PRONUN_ANOMALIES = "pronun.calc.anomalies";
        public const string PRONUN_IS_VALID = "pronun.calc.is_valid";

        public const string WORD_FORM_GRAPHICAL_FORM = "word_form.graphical_form";
        public const string WORD_FORM_LEMMA = "word_form.lemma";
        public const string WORD_FORM_POS = "word_form.pos";
        public const string WORD_FORM_GENDER = "word_form.gender";
        public const string WORD_FORM_NUMBER = "word_form.number";

        public const string LEMMA_GRAPHICAL_FORM = "lemma.graphical_form";
        public const string LEMMA_POS = "lemma.pos";
        public const string LEMMA_GENDER = "lemma.gender";

        public const string PHONETIC_SEQUENCE_IPA = "phon_seq.ipa";
        public const string PHONETIC_SEQUENCE_PHONEMES = "phon_seq.phonemes";
        public const string PHONETIC_SEQUENCE_SYLLABLES = "phon_seq.syllables";
    }

}
