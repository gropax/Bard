"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TokenType = exports.Token = exports.WordSpan = exports.VerseSpan = exports.ParagraphSpan = exports.TokenizedText = void 0;
var TokenizedText = /** @class */ (function () {
    function TokenizedText(text, paragraphs) {
        this.text = text;
        this.paragraphs = paragraphs;
    }
    return TokenizedText;
}());
exports.TokenizedText = TokenizedText;
var ParagraphSpan = /** @class */ (function () {
    function ParagraphSpan(startIndex, endIndex, verses) {
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.verses = verses;
    }
    return ParagraphSpan;
}());
exports.ParagraphSpan = ParagraphSpan;
var VerseSpan = /** @class */ (function () {
    function VerseSpan(startIndex, endIndex, words) {
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.words = words;
    }
    return VerseSpan;
}());
exports.VerseSpan = VerseSpan;
var WordSpan = /** @class */ (function () {
    function WordSpan(startIndex, endIndex) {
        this.startIndex = startIndex;
        this.endIndex = endIndex;
    }
    return WordSpan;
}());
exports.WordSpan = WordSpan;
var Token = /** @class */ (function () {
    function Token(startIndex, endIndex, type, newlineConut) {
        if (newlineConut === void 0) { newlineConut = 0; }
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.type = type;
        this.newlineConut = newlineConut;
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