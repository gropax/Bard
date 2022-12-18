
export enum TokenType {
  Blank,
  Word,
  Dash,
  Apostrophe,
  Punctuation,
  //NewVerse,
  //NewParagraph,
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

export abstract class TokenSpan {
  constructor(
    public tokens: Token[]) {
  }

  public content = this.tokens.map(t => t.content).join('');
  public initialToken = this.tokens[0];
  public lastToken = this.tokens[this.tokens.length-1];
}

export class Strophe extends TokenSpan {
  constructor(
    tokens: Token[],
    public verses: Verse[],
    public words: Word[])
  {
    super(tokens);
  }
}

export class Verse extends TokenSpan {
  constructor(
    tokens: Token[])
  {
    super(tokens);
  }
}

export class Word extends TokenSpan {
  constructor(
    tokens: Token[])
  {
    super(tokens);
  }
}
