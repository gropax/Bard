import { Injectable } from '@angular/core';
import { Strophe, Token, TokenType, Verse, Word, WordPart } from '../models/text';

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
        var wordParts = this.alignWithWords(words, verseTokens);
        verses.push(new Verse(verseTokens, wordParts));
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
        }
        words.push([token]);
        wordTokens = [];
      }
      else
        wordTokens.push(token);

      prevToken = token;
    }

    if (wordTokens.length > 0)
      words.push(wordTokens);

    return words;
  }

  private alignWithWords(words: Word[], verseTokens: Token[]) {
    var verseFirstToken = verseTokens[0];
    var verseLastToken = verseTokens[verseTokens.length-1];

    var wordParts = [];

    for (var word of words) {
      if (word.lastToken.endChar > verseFirstToken.startChar &&
          word.initialToken.startChar < verseLastToken.endChar)
      {
        var firstTokenIndex = Math.max(word.initialToken.index, verseFirstToken.index);
        var lastTokenIndex = Math.min(word.lastToken.index, verseLastToken.index);

        console.log(`word = ${word.content}`);
        console.log(`firstTokenIndex = ${firstTokenIndex}`);
        console.log(`lastTokenIndex = ${lastTokenIndex}`);

        var tokens = verseTokens.filter(t => firstTokenIndex <= t.index && t.index <= lastTokenIndex);

        wordParts.push(new WordPart(tokens, word));
      }
    }

    console.log(wordParts);

    return wordParts;
  }

}
