"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
exports.Word = exports.Verse = exports.Strophe = exports.TokenSpan = exports.Token = exports.TokenType = void 0;
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
var TokenSpan = /** @class */ (function () {
    function TokenSpan(tokens) {
        this.tokens = tokens;
        this.content = this.tokens.map(function (t) { return t.content; }).join('');
        this.initialToken = this.tokens[0];
        this.lastToken = this.tokens[this.tokens.length - 1];
    }
    return TokenSpan;
}());
exports.TokenSpan = TokenSpan;
var Strophe = /** @class */ (function (_super) {
    __extends(Strophe, _super);
    function Strophe(tokens, verses, words) {
        var _this = _super.call(this, tokens) || this;
        _this.verses = verses;
        _this.words = words;
        return _this;
    }
    return Strophe;
}(TokenSpan));
exports.Strophe = Strophe;
var Verse = /** @class */ (function (_super) {
    __extends(Verse, _super);
    function Verse(tokens) {
        return _super.call(this, tokens) || this;
    }
    return Verse;
}(TokenSpan));
exports.Verse = Verse;
var Word = /** @class */ (function (_super) {
    __extends(Word, _super);
    function Word(tokens) {
        return _super.call(this, tokens) || this;
    }
    return Word;
}(TokenSpan));
exports.Word = Word;
//# sourceMappingURL=text.js.map