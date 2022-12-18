import { Component, Input, OnChanges, OnInit, Renderer2, SimpleChanges } from '@angular/core';
import { Strophe, Token, TokenSpan, Word } from '../../../models/text';

@Component({
  selector: 'app-token-view',
  templateUrl: './token-view.component.html',
  styleUrls: ['./token-view.component.less']
})
export class TokenViewComponent implements OnInit, OnChanges {

  @Input() strophes: Strophe[];
  public segments: Segment[][][];  // strophe / verse / segment

  public hoveredWord: Word | null = null;
  public selectedWord: Word | null = null;
  public selectedWordElem: HTMLSpanElement | null = null;

  constructor(
    private renderer: Renderer2)
  {
    this.renderer.listen('window', 'click', (e:Event) => {
      if (e.target !== this.selectedWordElem) {
        this.selectedWord = null;
        this.selectedWordElem = null;
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.segments = this.computeSegments(changes['strophes'].currentValue);
  }

  ngOnInit(): void {
  }

  computeSegments(strophes: Strophe[]): Segment[][][] {
    var stropheList = [];

    for (var strophe of strophes) {
      var verseList = []
      for (var verse of strophe.verses) {
        verseList.push(this.segmentVerse(verse.tokens, strophe.words));
      }
      stropheList.push(verseList);
    }

    return stropheList;
  }

  private segmentVerse(verseTokens: Token[], words: Word[]): Segment[] {
    var segments: Segment[] = [];

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
      if (tokenWord) {
        if (tokenWord !== currentWord && wordTokens.length > 0) {
          segments.push(new Segment(wordTokens, currentWord));
          wordTokens = [];
          currentWord = tokenWord;
        }
        wordTokens.push(token);
        currentWord = tokenWord;
      } else {
        if (wordTokens.length > 0) {
          segments.push(new Segment(wordTokens, currentWord));
          wordTokens = [];
        }
        segments.push(new Segment([token], null, this.normalizeContent(token)));
      }
    }

    if (wordTokens.length > 0)
      segments.push(new Segment(wordTokens, tokenWord));

    return segments;
  }

  private normalizeContent(token: Token): string {
    var str = token.content.trim();
    if (str.length === 0)
      return ' ';
    else
      return str;
  }


  public selectWord($event: [Word, HTMLSpanElement]) {
    this.selectedWord = $event[0];
    this.selectedWordElem = $event[1];
  }

  public hoverWord($event: Word | null) {
    this.hoveredWord = $event;
  }

}


export class Segment {
  constructor(
    public tokens: Token[],
    public word: Word | null,
    public content: string = '')
  {
    this.content = content || tokens.map(t => t.content).join('');
  }

  public isWord = this.word !== null;
}

//export interface IVerseSegment {
//  tokens: Token[];
//  content: string;
//  isWord: boolean;
//  asWordPart(): WordPart;
//}

//export class PunctSegment extends TokenSpan implements IVerseSegment {
//  constructor(
//    public tokens: Token[],
//    public content: string)
//  {
//    super(tokens);
//  }

//  public isWord = false;
//  public asWordPart(): WordPart { throw "not a WordPart"; }
//}

//export class WordPart implements IVerseSegment {
//  constructor(
//    public tokens: Token[],
//    public word: Word) {
//  }

//  public isWord = true;
//  public asWordPart(): WordPart { return this; }

//  public content = this.tokens.map(t => t.content).join('');

//  public initialToken = this.tokens[0];
//  public lastToken = this.tokens[this.tokens.length-1];

//  public containsBegining = this.initialToken == this.word.initialToken;
//  public containsEnd = this.lastToken == this.word.lastToken;
//}
