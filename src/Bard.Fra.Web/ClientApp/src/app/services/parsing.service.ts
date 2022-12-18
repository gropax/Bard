import { Injectable } from '@angular/core';
import { IVerseSegment, PunctSegment, Strophe, Token, TokenType, Verse, Word, WordPart } from '../models/text';

@Injectable({
  providedIn: 'root'
})
export class ParsingService {

  public parse(tokens: Token[]): Strophe[] {
    var strophes = [];

    for (var stropheTokens of this.splitStrophes(tokens)) {
      var words = this.splitWords(stropheTokens).map(tx => new Word(tx));

      var verses = [];
      for (var verseTokens of this.splitVerses(stropheTokens)) {
        //var wordParts = this.alignWithWords(words, verseTokens);
        var segments = this.segmentVerse(verseTokens, words);
        verses.push(new Verse(verseTokens, segments));
      }

      strophes.push(new Strophe(stropheTokens, verses, words));
    }

    return strophes;
  }

  private splitStrophes(tokens: Token[]): Token[][] {
    var strophes = [];
    var stropheTokens = [];

    for (var i = 0; i < tokens.length; i++) {
      var token = tokens[i];

      // Ignore first token if blank
      if (i == 0 && token.type == TokenType.Blank)
        continue;

      if (token.type == TokenType.Blank && token.newlineCount >= 2) {
        strophes.push(stropheTokens);
        stropheTokens = [];
      }
      else
        stropheTokens.push(token);
    }

    if (stropheTokens.length > 0)
      strophes.push(stropheTokens);

    return strophes;
  }

  private splitVerses(tokens: Token[]): Token[][] {
    var verses = [];
    var verseTokens = [];

    for (var i = 0; i < tokens.length; i++) {
      var token = tokens[i];

      if (token.type == TokenType.Blank && token.newlineCount >= 1) {
        verses.push(verseTokens);
        verseTokens = [];
      }
      else
        verseTokens.push(token);
    }

    if (verseTokens.length > 0)
      verses.push(verseTokens);

    return verses;
  }

  private splitWords(tokens: Token[]): Token[][] {
    var words = [];
    var wordTokens = [];
    var prevToken = null;

    for (var i = 0; i < tokens.length; i++) {
      var token = tokens[i];

      if (token.type == TokenType.Blank) {
        if (token.newlineCount >= 1 && prevToken?.type == TokenType.Dash) {
          // If dash followed by newline, word is split on two lines
          // Ignore current blank token, keep adding tokens to current word
        } else {
          if (wordTokens.length > 0) {
            words.push(wordTokens);
          }
          wordTokens = [];
        }
      } else if (token.type == TokenType.Apostrophe) {
        wordTokens.push(token)
        words.push(wordTokens);
        wordTokens = [];
      } else if (token.type == TokenType.Punctuation) {
        if (wordTokens.length > 0) {
          words.push(wordTokens);
          wordTokens = [];
        }
        //words.push([token]);
        //wordTokens = [];
      }
      else
        wordTokens.push(token);

      prevToken = token;
    }

    if (wordTokens.length > 0)
      words.push(wordTokens);

    return words;
  }

  private segmentVerse(verseTokens: Token[], words: Word[]): IVerseSegment[] {
    var segments: IVerseSegment[] = [];

    var wordDict: { [tokenIndex: number]: Word } = {};
    for (var i = 0; i < words.length; i++) {
      var word = words[i];
      for (var j = 0; j < word.tokens.length; j++) {
        var token = word.tokens[j];
        wordDict[token.index] = word;
      }
    }

    var wordTokens: Token[] = [];
    var currentWord: any;
    var tokenWord: any;

    for (var i = 0; i < verseTokens.length; i++) {
      var token = verseTokens[i];

      tokenWord = wordDict[token.index];
      console.log(tokenWord);
      if (tokenWord) {
        wordTokens.push(token);
        currentWord = tokenWord;
      } else {
        if (wordTokens.length > 0) {
          segments.push(new WordPart(wordTokens, currentWord));
          wordTokens = [];
        }
        segments.push(new PunctSegment([token], this.normalizeContent(token)));
      }
    }

    if (wordTokens.length > 0)
      segments.push(new WordPart(wordTokens, tokenWord));

    return segments;
  }

  private normalizeContent(token: Token): string {
    var str = token.content.trim();
    if (str.length === 0)
      return ' ';
    else
      return str;
  }

}
