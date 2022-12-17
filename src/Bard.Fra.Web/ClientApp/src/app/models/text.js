"use strict";
//export class TokenizedText {
//  constructor(
//    public text: string,
//    public paragraphs: Strophe[]) {
//  }
//}
Object.defineProperty(exports, "__esModule", { value: true });
exports.TokenType = exports.Token = exports.Word = exports.Verse = exports.Strophe = void 0;
var Strophe = /** @class */ (function () {
    function Strophe(startIndex, endIndex, verses, words) {
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.verses = verses;
        this.words = words;
    }
    return Strophe;
}());
exports.Strophe = Strophe;
var Verse = /** @class */ (function () {
    function Verse(startIndex, endIndex, tokens) {
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.tokens = tokens;
    }
    return Verse;
}());
exports.Verse = Verse;
var Word = /** @class */ (function () {
    function Word(startIndex, endIndex, tokens) {
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.tokens = tokens;
    }
    return Word;
}());
exports.Word = Word;
var Token = /** @class */ (function () {
    function Token(startIndex, endIndex, type, content, newlineCount) {
        if (newlineCount === void 0) { newlineCount = 0; }
        this.startIndex = startIndex;
        this.endIndex = endIndex;
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