import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { BehaviorSubject, debounceTime, map } from 'rxjs';
import { Word } from '../../models/text';
import { ParsingService } from '../../services/parsing.service';
import { TokenizationService } from '../../services/tokenization.service';

@Component({
  selector: 'app-editor-page',
  templateUrl: './editor-page.component.html',
  styleUrls: ['./editor-page.component.less']
})
export class EditorPageComponent implements OnInit {

  public text: string;
  public textSubject = new BehaviorSubject<string>("");

  public $text = this.textSubject.asObservable().pipe(debounceTime(300));
  public $tokens = this.$text.pipe(map(text => this.tokenizer.tokenize(text)));
  public $strophes = this.$tokens.pipe(map(tokens => this.parser.parse(tokens)));

  public selectedWord: Word | null = null;
  public selectedWordElem: HTMLSpanElement | null = null;
  public highlightedWord: Word | null = null;

  public viewMode = {
    token: 'Tk',
    ipa: 'IPA',
  }
  public selectedViewMode: string = this.viewMode.token;

  constructor(
    private renderer: Renderer2,
    private tokenizer: TokenizationService,
    private parser: ParsingService)
  {
    this.renderer.listen('window', 'click', (e:Event) => {
      if (e.target !== this.selectedWordElem) {
        this.selectedWord = null;
        this.selectedWordElem = null;
      }
    });
  }

  ngOnInit(): void {
    //this.$text.subscribe(text => console.log(text));
    //this.$tokens.subscribe(tokens => console.log(tokens));
    //this.$strophes.subscribe(strophes => console.log(strophes));
  }

  public updateText() {
    this.textSubject.next(this.text);
  }

  public selectWord($event: [Word, HTMLSpanElement]) {
    this.selectedWord = $event[0];
    this.selectedWordElem = $event[1];
  }

  public selectViewMode(viewMode: string) {
    this.selectedViewMode = viewMode;
  }

}
