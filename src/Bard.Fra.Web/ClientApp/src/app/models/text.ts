
export class Strophe {
  constructor(
    public tokens: Token[],
    public verses: Verse[],
    public words: Word[]) {
  }

  public initialToken = this.tokens[0];
  public lastToken = this.tokens[this.tokens.length-1];
}

export class Verse {
  constructor(
    public tokens: Token[],
    public segments: IVerseSegment[]) {
  }

  public initialToken = this.tokens[0];
  public lastToken = this.tokens[this.tokens.length-1];
}


export interface IVerseSegment {
  tokens: Token[];
  content: string;
  isWord: boolean;
  asWordPart(): WordPart;
}

export class PunctSegment implements IVerseSegment {
  constructor(
    public tokens: Token[],
    public content: string) {
  }

  public isWord = false;
  public asWordPart(): WordPart { throw "not a WordPart"; }
}

export class WordPart implements IVerseSegment {
  constructor(
    public tokens: Token[],
    public word: Word) {
  }

  public isWord = true;
  public asWordPart(): WordPart { return this; }

  public content = this.tokens.map(t => t.content).join('');

  public initialToken = this.tokens[0];
  public lastToken = this.tokens[this.tokens.length-1];

  public containsBegining = this.initialToken == this.word.initialToken;
  public containsEnd = this.lastToken == this.word.lastToken;
}

export class Word {
  constructor(
    public tokens: Token[]) {
  }

  public content = this.tokens.map(t => t.content).join('');

  public initialToken = this.tokens[0];
  public lastToken = this.tokens[this.tokens.length-1];
}

export class Token {
  constructor(
    public index: number,
    public startChar: number,
    public endChar: number,
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
