"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TokenType = exports.Token = exports.Word = exports.WordPart = exports.PunctSegment = exports.Verse = exports.Strophe = void 0;
var Strophe = /** @class */ (function () {
    function Strophe(tokens, verses, words) {
        this.tokens = tokens;
        this.verses = verses;
        this.words = words;
        this.initialToken = this.tokens[0];
        this.lastToken = this.tokens[this.tokens.length - 1];
    }
    return Strophe;
}());
exports.Strophe = Strophe;
var Verse = /** @class */ (function () {
    function Verse(tokens, segments) {
        this.tokens = tokens;
        this.segments = segments;
        this.initialToken = this.tokens[0];
        this.lastToken = this.tokens[this.tokens.length - 1];
    }
    return Verse;
}());
exports.Verse = Verse;
var PunctSegment = /** @class */ (function () {
    function PunctSegment(tokens, content) {
        this.tokens = tokens;
        this.content = content;
        this.isWord = false;
    }
    PunctSegment.prototype.asWordPart = function () { throw "not a WordPart"; };
    return PunctSegment;
}());
exports.PunctSegment = PunctSegment;
var WordPart = /** @class */ (function () {
    function WordPart(tokens, word) {
        this.tokens = tokens;
        this.word = word;
        this.isWord = true;
        this.content = this.tokens.map(function (t) { return t.content; }).join('');
        this.initialToken = this.tokens[0];
        this.lastToken = this.tokens[this.tokens.length - 1];
        this.containsBegining = this.initialToken == this.word.initialToken;
        this.containsEnd = this.lastToken == this.word.lastToken;
    }
    WordPart.prototype.asWordPart = function () { return this; };
    return WordPart;
}());
exports.WordPart = WordPart;
var Word = /** @class */ (function () {
    function Word(tokens) {
        this.tokens = tokens;
        this.content = this.tokens.map(function (t) { return t.content; }).join('');
        this.initialToken = this.tokens[0];
        this.lastToken = this.tokens[this.tokens.length - 1];
    }
    return Word;
}());
exports.Word = Word;
var Token = /** @class */ (function () {
    function Token(index, startChar, endChar, type, content, newlineCount) {
        if (newlineCount === void 0) { newlineCount = 0; }
        this.index = index;
        this.startChar = startChar;
        this.endChar = endChar;
        this.type = type;
        this.content = content;
        this.newlineCount = newlineCount;
    }
    return Token;
}());
exports.Token = Token;
var TokenType;
(function (TokenType) {
    TokenType[TokenType["Blank"] = 0] = "Blank";
    TokenType[TokenType["Word"] = 1] = "Word";
    TokenType[TokenType["Dash"] = 2] = "Dash";
    TokenType[TokenType["Apostrophe"] = 3] = "Apostrophe";
    TokenType[TokenType["Punctuation"] = 4] = "Punctuation";
    //NewVerse,
    //NewParagraph,
})(TokenType = exports.TokenType || (exports.TokenType = {}));
//# sourceMappingURL=text.js.map