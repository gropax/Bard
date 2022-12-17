
//export class TokenizedText {
//  constructor(
//    public text: string,
//    public paragraphs: Strophe[]) {
//  }
//}

export class Strophe {
  constructor(
    public startIndex: number,
    public endIndex: number,
    public verses: Verse[],
    public words: Word[]) {
  }
}

export class Verse {
  constructor(
    public startIndex: number,
    public endIndex: number,
    public tokens: Token[]) {
  }
}

export class Word {
  constructor(
    public startIndex: number,
    public endIndex: number,
    public tokens: Token[]) {
  }
}

export class Token {
  constructor(
    public startIndex: number,
    public endIndex: number,
    public type: TokenType,
    public content: string,
    public newlineCount: number = 0) {
  }
}

export enum TokenType {
  Blank,
  Word,
  Dash,
  Apostrophe,
  Punctuation,
  //NewVerse,
  //NewParagraph,
}
