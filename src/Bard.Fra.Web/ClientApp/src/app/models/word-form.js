"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WordForm = void 0;
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
//# sourceMappingURL=word-form.js.map