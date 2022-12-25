"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.RhymingWords = exports.PhonGraphWord = exports.WordForm = exports.WordPronunciation = void 0;
var WordPronunciation = /** @class */ (function () {
    function WordPronunciation(id, graphemes, ipa) {
        this.id = id;
        this.graphemes = graphemes;
        this.ipa = ipa;
    }
    return WordPronunciation;
}());
exports.WordPronunciation = WordPronunciation;
var WordForm = /** @class */ (function () {
    function WordForm(id, graphemes, syllables, pos, number, gender, person, mood, tense) {
        this.id = id;
        this.graphemes = graphemes;
        this.syllables = syllables;
        this.pos = pos;
        this.number = number;
        this.gender = gender;
        this.person = person;
        this.mood = mood;
        this.tense = tense;
    }
    return WordForm;
}());
exports.WordForm = WordForm;
var PhonGraphWord = /** @class */ (function () {
    function PhonGraphWord(graphemes, syllables) {
        this.graphemes = graphemes;
        this.syllables = syllables;
    }
    return PhonGraphWord;
}());
exports.PhonGraphWord = PhonGraphWord;
var RhymingWords = /** @class */ (function () {
    function RhymingWords(graphemes, wordBeginning, rhyme) {
        this.graphemes = graphemes;
        this.wordBeginning = wordBeginning;
        this.rhyme = rhyme;
    }
    return RhymingWords;
}());
exports.RhymingWords = RhymingWords;
//# sourceMappingURL=word-form.js.map