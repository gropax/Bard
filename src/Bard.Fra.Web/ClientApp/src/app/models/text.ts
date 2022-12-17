
export class TokenizedText {
  constructor(
    public text: string,
    public paragraphs: ParagraphSpan[]) {
  }
}

export class ParagraphSpan {
  constructor(
    public startIndex: number,
    public endIndex: number,
    public verses: VerseSpan[]) {
  }
}

export class VerseSpan {
  constructor(
    public startIndex: number,
    public endIndex: number,
    public words: WordSpan[]) {
  }
}

export class WordSpan {
  constructor(
    public startIndex: number,
    public endIndex: number) {
  }
}

export class Token {
  constructor(
    public startIndex: number,
    public endIndex: number,
    public type: TokenType,
    public newlineConut: number = 0) {
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
