import { Injectable } from '@angular/core';
import { Token, TokenType } from '../models/text';

@Injectable({
  providedIn: 'root'
})
export class TokenizationService {

  public tokenize(text: string): Token[] {
    var tokens = [];

    var tokenType = TokenType.Blank;
    var newlineCount = 0;
    var startIndex = 0;

    for (var i = 0; i < text.length; i++) {
      var char = text[i];
      var charType = this.getCharType(char);

      if (tokenType == TokenType.Blank) {
        if (charType == CharType.Whitespace) {
          // next
        } else if (charType == CharType.Newline) {
          newlineCount++;
        } else {  
          if (i > 0)  // do not add empty blank token at start
            tokens.push(new Token(startIndex, i, tokenType, newlineCount));

          tokenType = this.getTokenType(charType);
          startIndex = i;
          newlineCount = 0;
        }

      } else if (tokenType == TokenType.Dash) {
        if (charType == CharType.Dash) {
          // next
        } else {
          tokens.push(new Token(startIndex, i, tokenType));
          tokenType = this.getTokenType(charType);
          startIndex = i;
        }

      } else if (tokenType == TokenType.Apostrophe) {
        if (charType == CharType.Apostrophe) {
          // next
        } else {
          tokens.push(new Token(startIndex, i, tokenType));
          tokenType = this.getTokenType(charType);
          startIndex = i;
        }

      } else if (tokenType == TokenType.Punctuation) {
        if (charType == CharType.Punctuation) {
          // next
        } else {
          tokens.push(new Token(startIndex, i, tokenType));
          tokenType = this.getTokenType(charType);
          startIndex = i;
        }
      
      } else {
        if (charType == CharType.Word) {
          // next
        } else {
          tokens.push(new Token(startIndex, i, tokenType));
          tokenType = this.getTokenType(charType);
          startIndex = i;
        }
      }
    }

    if (text.length > 0)
    tokens.push(new Token(startIndex, text.length, tokenType, newlineCount));

    return tokens;
  }

  private getTokenType(charType: CharType) {
    switch (charType) {
      case CharType.Whitespace:
      case CharType.Newline:
        return TokenType.Blank;
      case CharType.Dash:
        return TokenType.Dash;
      case CharType.Apostrophe:
        return TokenType.Apostrophe;
      case CharType.Punctuation:
        return TokenType.Punctuation;
      case CharType.Word:
      default:
        return TokenType.Word;
    }
  }

  private getCharType(char: string): CharType {
    if (this.isWhitespace(char))
      return CharType.Whitespace;
    else if (this.isNewline(char))
      return CharType.Newline;
    else if (this.isApostrophe(char))
      return CharType.Apostrophe;
    else if (this.isDash(char))
      return CharType.Dash;
    else if (this.isPunctuation(char))
      return CharType.Punctuation;
    else
      return CharType.Word;
  }

  whitespaceChars = [' ', '\r'];
  newlineChars = ['\n'];
  apostropheChars = ['\''];
  dashChars = ['-'];
  punctuationChars = ['.', ',', ';', ':', '"', '«', '»', '!', '?', '…'];

  isWhitespace(char: string) {
    return this.whitespaceChars.includes(char);
  }

  isNewline(char: string) {
    return this.newlineChars.includes(char);
  }

  isApostrophe(char: string) {
    return this.apostropheChars.includes(char);
  }

  isDash(char: string) {
    return this.dashChars.includes(char);
  }

  isPunctuation(char: string) {
    return this.punctuationChars.includes(char);
  }
}

enum CharType {
  Whitespace,
  Newline,
  Punctuation,
  Apostrophe,
  Dash,
  Word,
}
